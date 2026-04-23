namespace Webzine.WebApplication.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net.Http.Json;
    using System.Text.Json;

    public class ChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ChatController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Query([FromBody] QuestionRequest request)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("RagClient");
                var response = await httpClient.PostAsJsonAsync(
                    "/query",
                    new { question = request.Question }).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    return BadRequest(new { error = "Erreur lors de la communication avec le service RAG" });
                }

                var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return Content(result, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erreur serveur: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SetSource([FromBody] SourceRequest request)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("RagClient");
                var response = await httpClient.PostAsJsonAsync(
                    "/initialize",
                    new { source = request.Source }).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    using JsonDocument doc = JsonDocument.Parse(content);
                    var root = doc.RootElement;
                    var success = root.GetProperty("success").GetBoolean();

                    if (success)
                    {
                        return Ok(new { message = root.GetProperty("message").GetString() });
                    }
                    else
                    {
                        return BadRequest(new { error = root.GetProperty("message").GetString() });
                    }
                }

                return BadRequest(new { error = "Erreur lors de la communication avec le service RAG" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erreur serveur: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Status()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("RagClient");
                var response = await httpClient.GetAsync("/status").ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return Content(content, "application/json");
                }

                return StatusCode(500, new { error = "Impossible de récupérer le statut du RAG" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"Erreur serveur: {ex.Message}" });
            }
        }
    }

    public class QuestionRequest
    {
        public string Question { get; set; } = string.Empty;
    }

    public class SourceRequest
    {
        public string Source { get; set; } = "deezer";
    }
}