namespace Webzine.WebApplication.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Webzine.Entity.Fixtures;
    using Webzine.WebApplication.ViewModels;

    public class HomeController : Controller
    {
        /// <summary>
        /// Retourne la vue principale de l'application.
        /// </summary>
        /// <returns>Un objet IActionResult qui rend la vue associée à la page d'accueil.</returns>
        public IActionResult Home()
        {
            ListeTitreModel model = new ListeTitreModel
            {
                Titres = TitreFactory.CreerTitres(),
            };

            return this.View(model);
        }
    }
}
