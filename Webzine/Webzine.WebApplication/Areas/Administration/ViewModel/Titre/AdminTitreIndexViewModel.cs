namespace Webzine.WebApplication.Areas.Administration.ViewModel
{
    public class AdminTitreIndexViewModel
    {
        public IEnumerable<Webzine.Entity.Titre> PagedTitres { get; set; } = new List<Webzine.Entity.Titre>();
        public int CurrentPage { get; set; }
        public int MaxPage { get; set; }
    }
}
