using Microsoft.AspNetCore.Mvc;
using Webzine.Entity;
using Webzine.Entity.Fixtures;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("administration")]
    public class StyleController : Controller
    {
        [Route("styles")]
        public IActionResult Index()
        {
            var styles = LocalStylesFactory.CreerStyles();
            return View(styles);
        }

        [HttpGet]
        [Route("style/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("style/create")]
        public IActionResult Create(Style style)
        {
            return Ok(style);
        }
    }
}