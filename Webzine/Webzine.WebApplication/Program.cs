using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NLog;
using NLog.Web;
using Webzine.EntitiesContext;
using Webzine.EntitiesContext.Seeders;
using Webzine.Repository;
using Webzine.Repository.Contracts;
using Webzine.Repository.Db;
using Webzine.Repository.Local;

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

    // Ajoute les services n�cessaires pour permettfre l'utilisation des
    // controllers avec des vues.
    builder.Services.AddControllersWithViews()
        // Ajoute la compilation des vues lors de l'ex�cution de l'application.
        // Cela nous �vite de recompiler l'application � chaque modification de vue.
        // N�cessite le package Nuget Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.
        .AddRazorRuntimeCompilation();

    // Pour mettre les urls en minuscule
    builder.Services.AddRouting(options =>
    {
        options.LowercaseUrls = true;
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

    app.UseRouting();

    app.MapAreaControllerRoute(
    name: "admin_titres_list",
    areaName: "Administration",
    pattern: "administration/titres",
    defaults: new { controller = "Titre", action = "Index" });

    app.MapAreaControllerRoute(
    name: "admin_styles_list",
    areaName: "Administration",
    pattern: "administration/styles",
    defaults: new { controller = "Style", action = "Index" });

    app.MapAreaControllerRoute(
    name: "admin_artistes_list",
    areaName: "Administration",
    pattern: "administration/artistes",
    defaults: new { controller = "Artiste", action = "Index" });

    app.MapAreaControllerRoute(
    name: "admin_commentaires_list",
    areaName: "Administration",
    pattern: "administration/commentaires",
    defaults: new { controller = "Commentaire", action = "Index" });

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