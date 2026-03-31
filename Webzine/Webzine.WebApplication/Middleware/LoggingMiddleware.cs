using Microsoft.Extensions.Logging;

namespace Webzine.WebApplication.Middleware
{
    /// <summary>
    /// Middleware personnalisé pour la journalisation des requêtes HTTP.
    /// Enregistre la méthode HTTP, l'URL et le temps de réponse.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            var request = context.Request;

            _logger.LogInformation("[REQUÊTE] Méthode: {Method} | URL: {Path}{QueryString}", 
                request.Method, request.Path, request.QueryString);

            try
            {
                await _next(context);
            }
            finally
            {
                var duration = DateTime.UtcNow - startTime;
                var statusCode = context.Response.StatusCode;

                _logger.LogInformation("[RÉPONSE] Code: {StatusCode} | Durée: {DurationMs}ms | Méthode: {Method} | URL: {Path}{QueryString}", 
                    statusCode, duration.TotalMilliseconds, request.Method, request.Path, request.QueryString);
            }
        }
    }
}
