namespace Webzine.WebApplication.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Health check endpoint pour Docker, Kubernetes, load balancers
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HealthController> _logger;

        public HealthController(IHttpClientFactory httpClientFactory, ILogger<HealthController> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// Health check simple - vérifie si l'application est en cours d'exécution
        /// </summary>
        [HttpGet]
        [Route("")]
        [Route("live")]
        public IActionResult Live()
        {
            return Ok(new
            {
                status = "healthy",
                timestamp = DateTime.UtcNow,
                service = "Webzine"
            });
        }

        /// <summary>
        /// Ready check - vérifie si l'application est prête à traiter les demandes
        /// </summary>
        [HttpGet("ready")]
        public async Task<IActionResult> Ready()
        {
            try
            {
                // Vérifier la disponibilité du service RAG
                var httpClient = _httpClientFactory.CreateClient("RagClient");
                var ragResponse = await httpClient.GetAsync("/status", HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false);

                var ragHealthy = ragResponse.IsSuccessStatusCode;

                if (!ragHealthy)
                {
                    _logger.LogWarning("RAG service is not responding properly");
                    return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                    {
                        status = "not ready",
                        timestamp = DateTime.UtcNow,
                        service = "Webzine",
                        rag_status = "unhealthy"
                    });
                }

                return Ok(new
                {
                    status = "ready",
                    timestamp = DateTime.UtcNow,
                    service = "Webzine",
                    rag_status = "healthy"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during ready check");
                return StatusCode(StatusCodes.Status503ServiceUnavailable, new
                {
                    status = "not ready",
                    timestamp = DateTime.UtcNow,
                    service = "Webzine",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Detailed health check - retourne des informations détaillées sur tous les services
        /// </summary>
        [HttpGet("detailed")]
        public async Task<IActionResult> Detailed()
        {
            var healthInfo = new
            {
                timestamp = DateTime.UtcNow,
                application = new
                {
                    name = "Webzine",
                    version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "Unknown",
                    environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Unknown"
                },
                services = new { }
            };

            try
            {
                var httpClient = _httpClientFactory.CreateClient("RagClient");
                var ragResponse = await httpClient.GetAsync("/status", HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false);

                var ragHealthy = ragResponse.IsSuccessStatusCode;
                var ragMessage = ragHealthy ? "Healthy" : "Unhealthy";

                healthInfo = new
                {
                    healthInfo.timestamp,
                    healthInfo.application,
                    services = new
                    {
                        rag = new
                        {
                            status = ragMessage,
                            endpoint = "http://rag:8000"
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking RAG service health");
            }

            return Ok(healthInfo);
        }
    }
}
