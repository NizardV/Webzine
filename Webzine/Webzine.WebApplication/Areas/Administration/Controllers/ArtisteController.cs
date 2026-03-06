using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("administration")]
    public class ArtisteController : Controller
    {
        [Route("artistes")]
        public IActionResult Index()
        {
            var artistes = ArtisteFactory.CreerArtistes();
            return View(artistes);
        }
    }
}
