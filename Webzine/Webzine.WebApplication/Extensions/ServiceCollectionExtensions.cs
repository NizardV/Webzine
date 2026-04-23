using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;
using Webzine.Business.Contracts.Services;
using Webzine.Business.Services;
using Webzine.EntitiesContext;
using Webzine.Repository.Caching;
using Webzine.Repository.Contracts;
using Webzine.Repository.Contracts.Caching;
using Webzine.Repository.Db;
using Webzine.Repository.Deezer;
using Webzine.Repository.Local;
using Webzine.WebApplication.Filters;

namespace Webzine.WebApplication.Extensions
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configuration des Repositories (Local vs BDD)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="repoMode"></param>
        /// <param name="environment"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebzineRepositories(this IServiceCollection services, IConfiguration config, string repoMode, string environment)
        {
            if (repoMode == "Local")
            {
                var data = Webzine.EntitiesContext.Seeders.SeedDataLocal.Initialize();
                services.AddSingleton(data);

                services.AddScoped<IDashboardService, DashboardService>();
                services.AddScoped<ITitreRepository, LocalTitreRepository>();
                services.AddScoped<IArtisteRepository, LocalArtisteRepository>();
                services.AddScoped<IStyleRepository, LocalStyleRepository>();
                services.AddScoped<ICommentaireRepository, LocalCommentaireRepository>();
            }
            else if (repoMode == "Database")
            {
                if (environment == "Development")
                {
                    services.AddDbContext<WebzineDbContext>(options =>
                        options.UseSqlite(config.GetConnectionString("Sqlite")));
                }
                else if (environment == "Production")
                {
                    services.AddDbContext<WebzineDbContext>(options =>
                        options.UseNpgsql(config.GetConnectionString("Postgres")));
                }

                services.AddScoped<IDashboardService, DashboardService>();
                services.AddScoped<IArtisteRepository, DbArtisteRepository>();
                services.AddScoped<IStyleRepository, DbStyleRepository>();
                services.AddScoped<ICommentaireRepository, DbCommentaireRepository>();
                services.AddScoped<ITitreRepository, DbTitreRepository>();
            }

            return services;
        }

        /// <summary>
        /// Configuration des APIs externes (Deezer)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dataSource"></param>
        /// <param name="deezerApiBaseUrl"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IServiceCollection AddWebzineExternalServices(this IServiceCollection services, IConfiguration config, string dataSource, string deezerApiBaseUrl)
        {
            if (dataSource == "Deezer")
            {
                if (string.IsNullOrWhiteSpace(deezerApiBaseUrl))
                    throw new InvalidOperationException("La configuration 'AppConfig:DeezerApi' est requise.");

                services.AddScoped<ICacheService, MemoryCacheService>();
                services.AddScoped<IDeezerRepository, DeezerRepository>();
                services.AddHttpClient<IDeezerRepository, DeezerRepository>(client =>
                {
                    client.BaseAddress = new Uri(deezerApiBaseUrl);
                });
                services.AddScoped<IDeezerService, DeezerService>();
            }

            // Configuration du RAG
            var ragApiUrl = config.GetSection("AppConfig")["RagApi"];
            if (!string.IsNullOrWhiteSpace(ragApiUrl))
            {
                services.AddHttpClient("RagClient", client =>
                {
                    client.BaseAddress = new Uri(ragApiUrl);
                    client.Timeout = TimeSpan.FromSeconds(30);
                });
            }

            return services;
        }

        /// <summary>
        /// Configuration MVC, compression, rate limiting
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddWebzineWebConfigurations(this IServiceCollection services)
        {
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddMvcOptions(options => options.Filters.Add<ExceptionFilter>());

            services.AddScoped<ExceptionFilter>();

            services.AddResponseCompression(options => options.EnableForHttps = true);

            services.AddRateLimiter(options =>
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

            services.AddRouting(options => options.LowercaseUrls = true);

            return services;
        }
    }
}