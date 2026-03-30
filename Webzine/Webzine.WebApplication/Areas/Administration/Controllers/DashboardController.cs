using Microsoft.AspNetCore.Mvc;
using Webzine.Repository.Contracts;
using Webzine.WebApplication.Areas.Administration.ViewModel.Dashboard;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{

    /// <summary>
    /// Provides administrative dashboard functionality for the application, exposing endpoints to display aggregated
    /// statistics and key metrics related to titles, artists, and styles.
    /// </summary>
    /// <remarks>This controller is part of the 'Administration' area and is intended for use by authorized
    /// administrative users. It aggregates data from multiple repositories to present a summary view of the
    /// application's content and activity. The controller relies on dependency injection for repository access and is
    /// typically used to render the main dashboard page in the administration section.</remarks>
    [Area("Administration")]
    [Route("administration")]
    public class DashboardController : Controller
    {
        private readonly ITitreRepository _titreRepository;
        private readonly IArtisteRepository _artisteRepository;
        private readonly IStyleRepository _styleRepository;

        /// <summary>
        /// Initializes a new instance of the DashboardController class with the specified repositories.
        /// </summary>
        /// <param name="titreRepository">The repository used to access and manage titre data.</param>
        /// <param name="artisteRepository">The repository used to access and manage artiste data.</param>
        /// <param name="styleRepository">The repository used to access and manage style data.</param>
        public DashboardController(ITitreRepository titreRepository, IArtisteRepository artisteRepository, IStyleRepository styleRepository)
        {
            this._titreRepository = titreRepository;
            this._artisteRepository = artisteRepository;
            this._styleRepository = styleRepository;
        }

        /// <summary>
        /// Handles HTTP GET requests for the dashboard view, aggregating and presenting summary statistics about
        /// artists, titles, and styles.
        /// </summary>
        /// <remarks>The returned view model includes counts and summary information such as the number of
        /// artists, titles, styles, total likes, total plays, and highlights such as the most played title and the
        /// artist with the most albums. This action is typically used to provide an overview page for administrators or
        /// users with access to global statistics.</remarks>
        /// <returns>A view result that renders the dashboard page with aggregated data about artists, titles, and styles.</returns>
        [Route("dashboard")]
        public IActionResult Dashboard()
        {

            var titres = this._titreRepository.FindAll().ToList();
            var artistes = this._artisteRepository.FindAll().ToList();
            var styles = this._styleRepository.FindAll().ToList();

            var model = new DashboardViewModel
            {
                NbArtistes = artistes.Count,
                NbTitres = titres.Count,
                NbStyles = styles.Count,

                NbLikes = titres.Sum(t => t.NbLikes),
                NbLectures = titres.Sum(t => t.NbLectures),

                // On compte les artistes dont la biographie n'est ni vide, ni nulle, Count attend un bool
                NbBiographiesArtistes = artistes.Count(a => a.Biographie != null && a.Biographie != ""),

                TitrePlusLu = titres.OrderByDescending(t => t.NbLectures).FirstOrDefault().Libelle ?? "Aucun",

                ArtistesPlusChroniques = artistes
                .OrderByDescending(a => titres.Count(t => t.IdArtiste == a.IdArtiste && !string.IsNullOrWhiteSpace(t.Chronique)))
                .FirstOrDefault()?.Nom ?? "Aucun",


                ArtistePlusAlbums = artistes
                    .OrderByDescending(a => titres.Where(t => t.IdArtiste == a.IdArtiste).Select(t => t.Album).Distinct().Count())
                    .FirstOrDefault()?.Nom ?? "Aucun",
            };


            return this.View(model);
        }
    }
}
