using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;

namespace Webzine.WebApplication.Controllers
{
    /// <summary>
    /// Classe controller avec des vues pour les artistes.
    /// </summary>
    public class ArtisteController : Controller
    {
        [HttpGet]
        [Route("Artiste/{nom}")]
        public IActionResult DetailsArtiste(string nom)
        {
            var artiste = ArtisteFactory.TrouverParNom(nom);
            return View(artiste);
        }
    }
}
