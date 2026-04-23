namespace Webzine.Business.Contracts.Services
{
    public interface IRagService
    {
        Task<RagResponse> QueryAsync(string question);
        Task<bool> InitializeWithDeezerDataAsync(IDeezerService deezerService);
    }
}
