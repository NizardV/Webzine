using Webzine.Entity;

namespace Webzine.WebApplication.ViewModels
{
    public class SearchResultsModel
    {
        public string Query { get; set; } = string.Empty;

        public List<Artiste> Artistes { get; set; } = new();

        public List<Titre> Titres { get; set; } = new();
    }
}
