using Webzine.Entity;

namespace Webzine.WebApplication.ViewModels
{
    public class ArtisteModel
    {
        public Artiste Artiste { get; set; }
        public IEnumerable<Titre> Titres { get; set; }

        public List<AlbumGroupModel> Albums { get; set; } = new();
    }
}
