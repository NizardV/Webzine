namespace Webzine.Business.Contracts.Services
{
    public class RagResponse
    {
        public string Answer { get; set; } = string.Empty;
        public List<string> Sources { get; set; } = new();
    }
}
