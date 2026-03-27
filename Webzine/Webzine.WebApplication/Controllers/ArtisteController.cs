using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.Entity;
using Webzine.Repository.Contracts;
using Webzine.WebApplication.ViewModels;

namespace Webzine.WebApplication.Controllers
{
    /// <summary>
    /// Classe controller avec des vues pour les artistes.
    /// </summary>
    public class ArtisteController : Controller
    {
        private readonly IArtisteRepository _artisteRepository;
        private readonly ITitreRepository _titreRepository;

        public ArtisteController(IArtisteRepository artisteRepository, ITitreRepository titreRepository)
        {
            _artisteRepository = artisteRepository;
            _titreRepository = titreRepository;
        }
        /// <summary>
        /// Retourne la vue Artiste.cshtml avec le nom passé en paramètre.
        /// </summary>
        /// <returns>Une Vue</returns>
        [HttpGet("/artiste/{nom}")]
        public IActionResult DetailsArtiste(string nom)
        {
            var artiste = _artisteRepository.FindByName(nom);
            if (artiste == null)
            {
                return NotFound();
            }

            var titres = _titreRepository.FindAll().Where(t =>t.IdArtiste == artiste.IdArtiste);

            var model = new ArtisteModel { Artiste = artiste, Titres = titres };

            // Titres regroupés par nom d'album et URL de jaquette
            model.Albums = model.Titres
                .GroupBy(t => new { t.Album, t.UrlJaquette })
                // Transformez chaque groupe en un AlbumGroupModel
                .Select(g => new AlbumGroupModel
                {
                    Album = g.Key.Album ?? "Album",
                    UrlJaquette = g.Key.UrlJaquette ?? "",
                    Titres = g.ToList()
                })
                .ToList();

            return View(model);
        }
    }
}
