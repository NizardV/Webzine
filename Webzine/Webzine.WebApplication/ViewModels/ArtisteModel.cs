using Webzine.Entity;

namespace Webzine.WebApplication.ViewModels
{
    public class ArtisteModel
    {
        public Artiste Artiste { get; set; }
        public List<Titre> Titres { get; set; } = new();

        public List<AlbumGroupModel> Albums { get; set; } = new();
    }
}
