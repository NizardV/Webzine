using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.WebApplication.ViewModels;

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
        [HttpGet("/artiste/{nom}")]
        public IActionResult DetailsArtiste(string nom)
        {
            var artiste = ArtisteFactory.TrouverParNom(nom);
            if (artiste == null)
            {
                return NotFound();
            }

            var titres = TitreFactory.TrouverParIdArtiste(artiste.IdArtiste);

            var model = new ArtisteModel { Artiste = artiste, Titres = titres };

            return View(model);
        }
    }
}
