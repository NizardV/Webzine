using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices.JavaScript;
using Webzine.Entity;
using Webzine.Entity.Fixtures;
using Webzine.Repository.Contracts;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("administration")]
    public class StyleController : Controller
    {
        private readonly IStyleRepository _styleRepository;

        public StyleController(IStyleRepository styleRepository)
        {
            _styleRepository = styleRepository;
        }

        [Route("styles")]
        public IActionResult Index()
        {
            var styles = _styleRepository.FindAll();
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
            var style = new List<Style>(); // Ajouter la logique pour recupérer à partir du repo

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

        [HttpGet]
        [Route("style/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var style = new Style { }; // Ajouter la logique pour recupérer à partir du repo

            if (style == null)
            {
                return NotFound();
            }

            return View(style);
        }

        [HttpPost]
        [Route("style/delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {

            return RedirectToAction(nameof(Index));
        }

    }
}