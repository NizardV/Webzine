namespace Webzine.WebApplication.ViewModels
{
    using Webzine.Entity;

    public class ListeTitreModel
    {
        /// <summary>
        /// Obtient ou définit la liste des titres à afficher dans la vue.
        /// </summary>
        public List<Titre> Titres { get; set; }
    }
}
