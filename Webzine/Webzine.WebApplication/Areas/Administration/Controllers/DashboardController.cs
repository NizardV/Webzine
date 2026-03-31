using Microsoft.AspNetCore.Mvc;
using Webzine.Business.Contracts.Services;
using Webzine.WebApplication.Areas.Administration.ViewModel.Dashboard;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{

    /// <summary>
    /// Provides administrative dashboard functionality for the application, including the display of key statistics and
    /// metrics relevant to administrators.
    /// </summary>
    /// <remarks>This controller is part of the Administration area and is intended for use by users with
    /// administrative privileges. It relies on an implementation of IDashboardService to retrieve dashboard data. The
    /// controller is typically accessed via the /Administration/Dashboard route.</remarks>
    [Area("Administration")]
    public class DashboardController : Controller
    {
        private readonly IDashboardService dashboardService;

        /// <summary>
        /// Initialise une nouvelle instance de la classe DashboardController avec le service de tableau de bord spécifié.
        /// </summary>
        /// <param name="dashboardService">Le service utilisé pour fournir les opérations liées au tableau de bord. Ne peut pas être null.</param>
        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        /// <summary>
        /// Génère la vue principale du tableau de bord avec les statistiques actuelles de l'application.
        /// </summary>
        /// <remarks>Utilise le service de tableau de bord pour récupérer les statistiques et les transmet
        /// à la vue via un modèle de vue. Cette action est généralement utilisée comme point d'entrée pour l'affichage
        /// du tableau de bord administratif.</remarks>
        /// <returns>Un objet <see cref="IActionResult"/> qui rend la vue du tableau de bord avec les données statistiques
        /// associées.</returns>
        public IActionResult Index()
        {

            var stats = this.dashboardService.GetDashboardStats();

            var model = new DashboardViewModel
            {
                NbTitres = stats.NbTitres,
                TitrePlusLu = stats.TitrePlusLu,
                NbLikes = stats.NbLikes,
                NbLectures = stats.NbLectures,

                NbArtistes = stats.NbArtistes,
                NbBiographiesArtistes = stats.NbBiographiesArtistes,
                ArtistesPlusChroniques = stats.ArtistesPlusChroniques,
                ArtistePlusAlbums = stats.ArtistePlusAlbums,

                NbStyles = stats.NbStyles,
            };

            return this.View(model);
        }
    }
}
