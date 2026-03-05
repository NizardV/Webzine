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

        // Affiche le formulaire d'édition pré-rempli
        [HttpGet]
        [Route("style/edit/{id}")]
        public IActionResult Edit(int id) {
            var style = LocalStylesFactory.CreerStyles().FirstOrDefault(s => s.IdStyle == id);

            if (style == null) { 
                return NotFound();
            }

            return View(style);
        }

        [HttpPost]
        [Route("style/edit/{id}")]
        public IActionResult Edit(int id, Style style)
        {
            if (id != style.IdStyle)
            {
                return BadRequest();
            }
            return Ok(style);
        }
    }
}