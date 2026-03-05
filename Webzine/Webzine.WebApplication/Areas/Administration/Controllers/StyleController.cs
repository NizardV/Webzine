using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    [Area("Administration")]
    public class StyleController : Controller
    {
        // La route par défaut de ce contrôleur sera donc /administration/styles
        [Route("administration/styles")]
        public IActionResult Index()
        {
            var styles = LocalStylesFactory.CreerStyles();
            return View(styles);
        }
    }
}
