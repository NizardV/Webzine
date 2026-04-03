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

        /// <summary>
        /// Initializes a new instance of the ArtisteController class with the specified repositories.
        /// </summary>
        /// <param name="artisteRepository">The repository used to access and manage artiste data. Cannot be null.</param>
        /// <param name="titreRepository">The repository used to access and manage titre data. Cannot be null.</param>
        public ArtisteController(IArtisteRepository artisteRepository, ITitreRepository titreRepository)
        {
            this._artisteRepository = artisteRepository;
            this._titreRepository = titreRepository;
        }

        /// <summary>
        /// Retourne la vue Artiste.cshtml avec le nom passé en paramètre.
        /// </summary>
        /// <returns>Une Vue</returns>
        public IActionResult DetailsArtiste(string nom)
        {
            var artiste = this._artisteRepository.FindByName(nom);
            if (artiste == null)
            {
                return this.NotFound();
            }

            var model = new ArtisteModel { Artiste = artiste, Titres = artiste.Titres };

            // Titres regroupés par nom d'album et URL de jaquette
            model.Albums = model.Titres
                .GroupBy(t => new { t.Album, t.UrlJaquette })
                // Transformez chaque groupe en un AlbumGroupModel
                .Select(g => new AlbumGroupModel
                {
                    Album = g.Key.Album ?? "Album",
                    UrlJaquette = g.Key.UrlJaquette ?? "",
                    Titres = g.ToList(),
                })
                .ToList();

            return this.View(model);
        }
    }
}
