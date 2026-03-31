using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Webzine.WebApplication.Filters
{
    /// <summary>
    /// Filtre d'exception global qui intercepte les exceptions non gérées
    /// et retourne une réponse JSON structurée.
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;
        private readonly IWebHostEnvironment _environment;

        public ExceptionFilter(ILogger<ExceptionFilter> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "Exception non gérée interceptée");

            var isDevelopment = _environment.IsDevelopment();

            var response = new
            {
                statusCode = StatusCodes.Status500InternalServerError,
                message = "Une erreur interne est survenue.",
                errorCode = "INTERNAL_ERROR",
                timestamp = DateTime.UtcNow,
                details = isDevelopment ? context.Exception.Message : null,
                stackTrace = isDevelopment ? context.Exception.StackTrace : null
            };

            context.Result = new JsonResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }
    }
}
