using Webzine.Entity;

namespace Webzine.WebApplication.ViewModels
{
    public class AlbumGroupModel
    {
        public string Album { get; set; }
        public string UrlJaquette { get; set; }
        public List<Titre> Titres { get; set; } = new();

    }
}
