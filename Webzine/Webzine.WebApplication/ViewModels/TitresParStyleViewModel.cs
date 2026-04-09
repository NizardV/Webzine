using Webzine.Entity;

namespace Webzine.WebApplication.ViewModels
{

    public class TitresParStyleViewModel
    {
        public string NomDuStyle { get; set; }
        public IEnumerable<Titre> Titres { get; set; }
    }
}
