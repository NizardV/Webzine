namespace Webzine.WebApplication.Areas.Administration.ViewModel.Artiste
{
    public class AdminArtisteIndexViewModel
    {
        public IEnumerable<Webzine.Entity.Artiste> PagedArtistes { get; set; } = new List<Webzine.Entity.Artiste>();
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
    }
}
