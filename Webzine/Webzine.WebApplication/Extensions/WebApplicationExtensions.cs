using Webzine.EntitiesContext;
using Webzine.EntitiesContext.Seeders;
using Webzine.Business.Contracts.Services;
using Webzine.WebApplication.Middleware;
using Microsoft.AspNetCore.Mvc;

using AspNetApp = Microsoft.AspNetCore.Builder.WebApplication;

namespace Webzine.WebApplication.Extensions
{
    public static class WebApplicationExtensions
    {
        /// <summary>
        /// L'ensemencement de la base (Seeding)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="repoMode"></param>
        /// <param name="dataSource"></param>
        /// <param name="logger"></param>
        public static void InitializeDatabase(this AspNetApp app, string repoMode, string dataSource, NLog.Logger logger)
        {
            if (repoMode != "Database") return;

            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<WebzineDbContext>();

            context.Database.EnsureCreated();

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
        }

        /// <summary>
        /// Configuration des Middlewares
        /// </summary>
        /// <param name="app"></param>
        public static void ConfigureWebzineMiddlewares(this AspNetApp app)
        {
            app.UseStaticFiles();
            app.UseRateLimiter();
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseMiddleware<SecurityHeadersMiddleware>();
            app.UseMiddleware<LoggingMiddleware>();
            app.UseStatusCodePagesWithReExecute("/Error/Index", "?statusCode={0}");
            app.UseRouting();
        }

        /// <summary>
        /// Configuration des routes
        /// </summary>
        /// <param name="app"></param>
        public static void MapWebzineRoutes(this AspNetApp app)
        {
            // Routes API Controllers
            app.MapControllers();

            // Health check routes (accessible depuis /health ou /api/health)
            app.MapGet("/health", () =>
            {
                return Results.Ok(new
                {
                    status = "healthy",
                    timestamp = DateTime.UtcNow,
                    service = "Webzine"
                });
            });

            // Routes Publiques
            app.MapControllerRoute(name: "public_titre_details", pattern: "titre/{id}", defaults: new { controller = "Titre", action = "Details" });
            app.MapControllerRoute(name: "public_titres_style", pattern: "titre/style/{libelle}", defaults: new { controller = "Titre", action = "Style" });
            app.MapControllerRoute(name: "public_artiste_details", pattern: "artiste/{nom}", defaults: new { controller = "Artiste", action = "DetailsArtiste" });

            // Routes Administration
            app.MapAreaControllerRoute(name: "admin_titres_list", areaName: "Administration", pattern: "administration/titres", defaults: new { controller = "Titre", action = "Index" });
            app.MapAreaControllerRoute(name: "admin_styles_list", areaName: "Administration", pattern: "administration/styles", defaults: new { controller = "Style", action = "Index" });
            app.MapAreaControllerRoute(name: "admin_artistes_list", areaName: "Administration", pattern: "administration/artistes", defaults: new { controller = "Artiste", action = "Index" });
            app.MapAreaControllerRoute(name: "admin_commentaires_list", areaName: "Administration", pattern: "administration/commentaires", defaults: new { controller = "Commentaire", action = "Index" });

            // Routes par défaut
            app.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}