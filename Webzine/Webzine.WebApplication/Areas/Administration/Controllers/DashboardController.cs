using Microsoft.AspNetCore.Mvc;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("administration")]
    public class DashboardController : Controller
    {
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            ViewBag.Artistes = 660;
            ViewBag.ArtistePlusChronique = 747;
            ViewBag.nombreBiographiesArtistes = 101;
            ViewBag.nombreStyles = 31;
            ViewBag.TitrePlusLu = "Paul Johnson - Get Get Down - Radio Edit";
            ViewBag.NombreLectures = 193992;
            ViewBag.NombreLikes = 29331;
            ViewBag.ArtistePlusAlbums = "DJ Hell";
            ViewBag.NombreTitres = 840;
            return View();
        }
    }
}
