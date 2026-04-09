using NLog;
using NLog.Web;
using Webzine.WebApplication.Extensions;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    logger.Info("Démarrage de l'application Webzine.");
    var builder = WebApplication.CreateBuilder(args);

    // --- 1. CONFIGURATION ---
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var config = builder.Configuration.GetSection("AppConfig");
    var environment = config["Environment"];
    var repoMode = config["Repository"];
    var dataSource = config["Seeder"];
    var deezerApiBaseUrl = config["DeezerApi"];

    logger.Info("Configuration chargée - Env: {Environment}, Repo: {RepoMode}, Seeder: {DataSource}", environment, repoMode, dataSource);

    // --- 2. INJECTION DES DÉPENDANCES (Nos méthodes d'extension) ---
    builder.Services.AddWebzineRepositories(builder.Configuration, repoMode, environment);
    builder.Services.AddWebzineExternalServices(dataSource, deezerApiBaseUrl);
    builder.Services.AddWebzineWebConfigurations();

    // --- 3. CRÉATION DE L'APPLICATION ---
    var app = builder.Build();
    logger.Info("Application construite avec succès.");

    // --- 4. PIPELINE ET ROUTAGE (Nos méthodes d'extension) ---
    app.InitializeDatabase(repoMode, dataSource, logger);
    app.ConfigureWebzineMiddlewares();
    app.MapWebzineRoutes();

    // --- 5. LANCEMENT ---
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