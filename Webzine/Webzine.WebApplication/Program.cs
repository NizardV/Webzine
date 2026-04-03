using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System.Threading.RateLimiting;
using Webzine.Business.Contracts.Services;
using Webzine.Business.Services;
using Webzine.EntitiesContext;
using Webzine.EntitiesContext.Seeders;
using Webzine.Repository.Caching;
using Webzine.Repository.Contracts;
using Webzine.Repository.Contracts.Caching;
using Webzine.Repository.Db;
using Webzine.Repository.Deezer;
using Webzine.Repository.Local;
using Webzine.WebApplication.Filters;
using Webzine.WebApplication.Middleware;

var logger = LogManager.Setup()
  .LoadConfigurationFromAppSettings()
  .GetCurrentClassLogger();

try
{
    logger.Info("Démarrage de l'application Webzine.");

    var builder = WebApplication.CreateBuilder(args);

    // Configuration de NLog
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var config = builder.Configuration.GetSection("AppConfig");
    var environment = config["Environment"];
    var repoMode = config["Repository"];
    var dataSource = config["Seeder"];
    var deezerApiBaseUrl = config["DeezerApi"];

    logger.Info("Configuration chargée - Environnement: {Environment}, Repository: {RepoMode}, Seeder: {DataSource}", environment, repoMode, dataSource);

    if (repoMode == "Local")
    {

        var data = SeedDataLocal.Initialize();

        //Ajout du singleton pour les données locales
        builder.Services.AddSingleton(data);

        // Injecter les repo locaux
        builder.Services.AddScoped<IDashboardService, DashboardService>();
        builder.Services.AddScoped<ITitreRepository, LocalTitreRepository>();
        builder.Services.AddScoped<IArtisteRepository, LocalArtisteRepository>();
        builder.Services.AddScoped<IStyleRepository, LocalStyleRepository>();
        builder.Services.AddScoped<ICommentaireRepository, LocalCommentaireRepository>();

    }
    else if (repoMode == "Database")
    {
        // En fonction de l'environnement on ajoute un dbContext différent
        if (environment == "Development")
        {
            builder.Services.AddDbContext<WebzineDbContext>(options =>
              options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));

            logger.Info("WebzineDbContext configuré avec SQLite pour l'environnement Development.");
        }
        else if (environment == "Production")
        {
            builder.Services.AddDbContext<WebzineDbContext>(options =>
              options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));

            logger.Info("WebzineDbContext configuré avec PostgreSQL pour l'environnement Prod.");
        }

        // Injection des repos DB
        builder.Services.AddScoped<IDashboardService, DashboardService>();
        builder.Services.AddScoped<IArtisteRepository, DbArtisteRepository>();
        builder.Services.AddScoped<IStyleRepository, DbStyleRepository>();
        builder.Services.AddScoped<ICommentaireRepository, DbCommentaireRepository>();
        builder.Services.AddScoped<ITitreRepository, DbTitreRepository>();
    }
    if (dataSource == "Deezer")
    {
        if (string.IsNullOrWhiteSpace(deezerApiBaseUrl))
        {
            throw new InvalidOperationException("La configuration 'AppConfig:DeezerApi' est requise quand la source de données est 'Deezer'. Veuillez vérifier votre fichier de configuration.");
        }

        builder.Services.AddScoped<ICacheService, MemoryCacheService>();
        builder.Services.AddScoped<IDeezerRepository, DeezerRepository>();
        builder.Services.AddHttpClient<IDeezerRepository, DeezerRepository>(client =>
        {
            client.BaseAddress = new Uri(deezerApiBaseUrl);
        });
        builder.Services.AddScoped<IDeezerService, DeezerService>();
    }
    // Ajoute les services n�cessaires pour permettfre l'utilisation des
    // controllers avec des vues.
    builder.Services.AddControllersWithViews()
      // Ajoute la compilation des vues lors de l'exécution de l'application.
      // Cela nous évite de recompiler l'application à chaque modification de vue.
      // Nécessite le package Nuget Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.
      .AddRazorRuntimeCompilation()
      // Enregistre le filtre d'exception global
      .AddMvcOptions(options =>
      {
          options.Filters.Add<ExceptionFilter>();
      });

    // Enregistre le filtre comme service pour l'injection de dépendances
    builder.Services.AddScoped<ExceptionFilter>();

    // Configure la compression des réponses
    builder.Services.AddResponseCompression(options =>
    {
        options.EnableForHttps = true;
    });

    // Configure le Rate Limiting
    builder.Services.AddRateLimiter(options =>
    {
        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
          RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Request.Path.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 2
            }));
        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    });

    // Pour mettre les urls en minuscule
    builder.Services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
    });

    var app = builder.Build();
    logger.Info("Application construite avec succès.");

    if (repoMode == "Database")
    {
        using
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WebzineDbContext>();

        var hasData = context.Titres.Any() || context.Artistes.Any() || context.Styles.Any() || context.Commentaires.Any();

        if (!hasData)
        {
            logger.Info("BDD vide. Recréation et seeding de la bdd");
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            if (dataSource == "Factories")
            {
                SeedDataDb.Initialize(context);
            }
            else if (dataSource == "Deezer")
            {
                var deezerService = scope.ServiceProvider.GetRequiredService<IDeezerService>();

                SeedDataDbDeezer.Seed(deezerService, context, logger);
            }
        }

        // Ajoute les fichiers statiques (Bootstrap, CSS, JS)
        app.UseStaticFiles();


        // Enregistre les middlewares dans le pipeline
        app.UseRateLimiter();
        app.UseHsts();
        app.UseHttpsRedirection();
        app.UseMiddleware<SecurityHeadersMiddleware>();
        app.UseMiddleware<LoggingMiddleware>();

        app.UseRouting();


        // route publique qui affiche les détails d'un titre
        app.MapControllerRoute(
        name: "public_titre_details",
        pattern: "titre/{id}",
        defaults: new { controller = "Titre", action = "Details" });

        // Route publique pour filtrer les titres par style
        app.MapControllerRoute(
            name: "public_titres_style",
            pattern: "titre/style/{libelle}",
            defaults: new { controller = "Titre", action = "Style" });

        // route publique qui va intercepter artiste/nom pour afficher les détails d'un artiste
        app.MapControllerRoute(
        name: "public_artiste_details",
        pattern: "artiste/{nom}",
        defaults: new { controller = "Artiste", action = "DetailsArtiste" });

        app.MapAreaControllerRoute(
        name: "admin_titres_list",
        areaName: "Administration",
        pattern: "administration/titres",
        defaults: new { controller = "Titre", action = "Index" });

        app.MapAreaControllerRoute(
          name: "admin_styles_list",
          areaName: "Administration",
          pattern: "administration/styles",
          defaults: new
          {
              controller = "Style",
              action = "Index"
          });

        app.MapAreaControllerRoute(
          name: "admin_artistes_list",
          areaName: "Administration",
          pattern: "administration/artistes",
          defaults: new
          {
              controller = "Artiste",
              action = "Index"
          });

        app.MapAreaControllerRoute(
          name: "admin_commentaires_list",
          areaName: "Administration",
          pattern: "administration/commentaires",
          defaults: new
          {
              controller = "Commentaire",
              action = "Index"
          });

        // Configure le routage pour les zones (areas)
        app.MapControllerRoute(
          name: "areas",
          pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        // Configure le routage par défaut
        app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");

        logger.Info("Application prête. Lancement du serveur.");
        app.Run();
    }
}
catch (Exception ex)
{
    logger.Fatal(ex, "Arrêt de l'application suite à une erreur fatale au démarrage.");
    throw;
}
finally
{
    LogManager.Shutdown();
}