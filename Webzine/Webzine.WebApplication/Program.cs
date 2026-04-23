using NLog;
using NLog.Web;
using Webzine.WebApplication.Extensions;

// Configure NLog with environment-specific config
var environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var nlogConfigFile = environment == "Development" ? "nlog.development.config" : "nlog.config";

var logger = LogManager.Setup()
    .LoadConfigurationFromFile(nlogConfigFile)
    .GetCurrentClassLogger();

try
{
    logger.Info("Démarrage de l'application Webzine (Environnement: {Environment}).", environment);
    var builder = WebApplication.CreateBuilder(args);

    // --- 1. CONFIGURATION ---
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var config = builder.Configuration.GetSection("AppConfig");
    var repoMode = config["Repository"];
    var dataSource = config["Seeder"];
    var deezerApiBaseUrl = config["DeezerApi"];

    logger.Info("Configuration chargée - Repo: {RepoMode}, Seeder: {DataSource}", repoMode, dataSource);

    // --- 2. INJECTION DES DÉPENDANCES (Nos méthodes d'extension) ---
    builder.Services.AddWebzineRepositories(builder.Configuration, repoMode, environment);
    builder.Services.AddWebzineExternalServices(builder.Configuration, dataSource, deezerApiBaseUrl);
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
    try
    {
        // Arrêter NLog avec un timeout court
        LogManager.Flush(TimeSpan.FromSeconds(2));
        LogManager.Shutdown();
    }
    catch (System.Threading.Tasks.TaskCanceledException)
    {
        System.Console.WriteLine("⚠️  NLog flush timeout");
    }
    catch (Exception ex)
    {
        System.Console.WriteLine($"❌ Erreur lors de l'arrêt de NLog: {ex.Message}");
    }
}