namespace Webzine.Business.Services
{
    using System.Net.Http.Json;
    using System.Text.Json;
    using Webzine.Business.Contracts.Services;

    public class RagService : IRagService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RagService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<RagResponse> QueryAsync(string question)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RagClient");
                var response = await client.PostAsJsonAsync("/query", new { question }).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    return JsonSerializer.Deserialize<RagResponse>(content) ?? new RagResponse();
                }

                return new RagResponse { Answer = "Erreur lors de la requête RAG" };
            }
            catch (Exception ex)
            {
                return new RagResponse { Answer = $"Erreur: {ex.Message}" };
            }
        }

        public async Task<bool> InitializeWithDeezerDataAsync(IDeezerService deezerService)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RagClient");

                // Récupérer tous les artistes et leurs données
                var artistsData = new List<string>();

                // Exemple: vous pouvez adapter selon vos besoins
                // Pour l'instant, on appelle juste le endpoint d'init
                var response = await client.PostAsync("/initialize", null).ConfigureAwait(false);

                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
