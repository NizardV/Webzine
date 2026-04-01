using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NLog;
using NLog.Web;
using System.Threading.RateLimiting;
using Webzine.EntitiesContext;
using Webzine.EntitiesContext.Seeders;
using Webzine.Repository;
using Webzine.Repository.Contracts;
using Webzine.Repository.Db;
using Webzine.Repository.Local;
using Webzine.WebApplication.Middleware;
using Webzine.WebApplication.Filters;

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
    var environment = config["Environment"] ?? "Development";
    var repoMode = config["Repository"] ?? "Local";
    var dataSource = config["Seeder"] ?? "Factories";

    logger.Info("Configuration chargée - Environnement: {Environment}, Repository: {RepoMode}, Seeder: {DataSource}", 
        environment, repoMode, dataSource);

    if (repoMode == "Local")
    {
        logger.Info("Mode de persistance sélectionné : Local.");

        var data = dataSource == "Factories"
            ? SeedDataLocal.Initialize()
            //TODO Remplacer par spotify au jalon 3
            : SeedDataLocal.Initialize();

        logger.Info(
            "Initialisation des données locales terminée avec la source {DataSource}.",
            string.IsNullOrWhiteSpace(dataSource) ? "par défaut" : dataSource);

        //Ajout du singleton pour les données locales
        builder.Services.AddSingleton(data);

        //Injecter les repo locaux
        builder.Services.AddScoped<ITitreRepository, LocalTitreRepository>();
        builder.Services.AddScoped<IArtisteRepository, LocalArtisteRepository>();
        builder.Services.AddScoped<IStyleRepository, LocalStyleRepository>();
        builder.Services.AddScoped<ICommentaireRepository, LocalCommentaireRepository>();

    }
    else if (repoMode == "Database")
    {
        logger.Info("Mode de persistance sélectionné : Database.");

        //En fonction de l'environnement on ajoute un dbContext différent
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

            logger.Info("WebzineDbContext configuré avec SQLite pour l'environnement Development.");
        }

        // Injection des repos DB
        //builder.Services.AddScoped<ITitreRepository, DbTitreRepository>();
        builder.Services.AddScoped<IArtisteRepository, DbArtisteRepository>();
        builder.Services.AddScoped<IStyleRepository, DbStyleRepository>();
        builder.Services.AddScoped<ICommentaireRepository, DbCommentaireRepository>();
        builder.Services.AddScoped<ITitreRepository, DbTitreRepository>();
    }

    // Ajoute les services nécessaires pour permettfre l'utilisation des
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

    var app = builder.Build();
    logger.Info("Application construite avec succès.");

    if (repoMode == "Database")
    {
        logger.Info("Vérification de l'initialisation de la base de données.");


        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WebzineDbContext>();

        if (dataSource == "Factories")
        {
            if (environment == "Development")
            {
                logger.Warn("Suppression et recréation de la base en environnement Development avant seeding.");

                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
                SeedDataDb.Initialize(context);

                logger.Info("Base SQLite recréée et seedée avec les données Factories.");

            }
        }
        //    else if (dataSource == "Spotify")
        //    {
        //        SeedDataSpotify.InitializeDb(context);
        //    }
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
catch (Exception ex)
{
    logger.Fatal(ex, "Arrêt de l'application suite à une erreur fatale au démarrage.");
    throw;
}
finally
{
    LogManager.Shutdown();
}