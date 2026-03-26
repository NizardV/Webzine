using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Webzine.EntitiesContext.Seeders;
using Webzine.Repository.Contracts;
using Webzine.Repository.Local;
using Webzine.EntitiesContext;
using Webzine.Repository.Db;
using Webzine.Repository;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration.GetSection("AppConfig");
var environment = config["Environment"];
var repoMode = config["RepositoryMode"];
var dataSource = config["DataSource"];

if (repoMode == "Local")
{
    var data = dataSource == "Factories"
        ? SeedDataLocal.Initialize()
        //TODO Remplacer par spotify au jalon 3
        : SeedDataLocal.Initialize();

    //Ajout du singleton pour les données locales
    builder.Services.AddSingleton(data);

    //Injecter les repo locaux
    builder.Services.AddScoped<ITitreRepository, LocalTitreRepository>();
    builder.Services.AddScoped<IArtisteRepository, LocalArtisteRepository>();
    builder.Services.AddScoped<IStyleRepository, LocalStyleRepository>();
    builder.Services.AddScoped<ICommentaireRepository, LocalCommentaireRepository>();

}
else if(repoMode == "Database")
{
    //En fonction de l'environnement on ajoute un dbContext différent
    if (environment == "Development")
    {
        builder.Services.AddDbContext<WebzineDbContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite")));
    }
    else if (environment == "Production")
    {
        builder.Services.AddDbContext<WebzineDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres")));
    }

    // Injection des repos DB
    //builder.Services.AddScoped<ITitreRepository, DbTitreRepository>();
}

// Ajoute les services n�cessaires pour permettfre l'utilisation des
// controllers avec des vues.
builder.Services.AddControllersWithViews()
    // Ajoute la compilation des vues lors de l'ex�cution de l'application.
    // Cela nous �vite de recompiler l'application � chaque modification de vue.
    // N�cessite le package Nuget Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation.
    .AddRazorRuntimeCompilation();

var app = builder.Build();

if (repoMode == "Database")
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<WebzineDbContext>();

    if (dataSource == "Factories")
    {
        if (environment == "Development")
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            SeedDataDb.Initialize(context);
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

// Configure le routage pour les zones (areas)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

// Configure le routage par défaut
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
