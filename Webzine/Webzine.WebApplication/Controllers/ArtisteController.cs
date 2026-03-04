using Microsoft.AspNetCore.Mvc;

namespace Webzine.WebApplication.Controllers
{
    /// <summary>
    /// Classe controller avec des vues pour les artistes.
    /// </summary>
    public class ArtisteController : Controller
    {
        /// <summary>
        /// Retourne la vue Artiste.cshtml avec le nom passé en paramètre.
        /// </summary>
        /// <returns>Une Vue</returns>
        public IActionResult Artiste(string nom)
        {



            return View();
        }
    }
}
