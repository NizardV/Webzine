using Microsoft.AspNetCore.Mvc;
using Webzine.Entity;
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

        [HttpGet]
        [Route("artiste/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("artiste/create")]
        public IActionResult Create(Artiste artiste)
        {
            return Ok(artiste);
        }

        // Affiche le formulaire d'édition pré-rempli
        [HttpGet]
        [Route("artiste/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var artiste = ArtisteFactory.CreerArtistes().FirstOrDefault(a => a.IdArtiste == id);

            if (artiste == null)
            {
                return NotFound();
            }

            return View(artiste);
        }

        [HttpPost]
        [Route("artiste/edit/{id}")]
        public IActionResult Edit(int id, Artiste artiste)
        {
            if (id != artiste.IdArtiste)
            {
                return BadRequest();
            }
            return Ok(artiste);
        }
    }
}
