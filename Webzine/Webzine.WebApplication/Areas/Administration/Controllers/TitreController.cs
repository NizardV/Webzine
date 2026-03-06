namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Webzine.Entity;
    using Webzine.Entity.Fixtures;

    [Area("Administration")]
    [Route("administration")]
    public class TitreController : Controller
    {
        [Route("titres")]
        public IActionResult Index()
        {
            var titres = TitreFactory.GetTitres();
            return View(titres);
        }

        [HttpGet]
        [Route("titre/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("titre/create")]
        public IActionResult Create(Titre titre)
        {
            return Ok(titre);
        }

        [HttpGet]
        [Route("titre/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var titre = TitreFactory.GetTitreById(id);
            if (titre == null)
            {
                return NotFound();
            }
            return View(titre);
        }

        [HttpPost]
        [Route("titre/edit/{id}")]
        public IActionResult Edit(int id, Titre titre)
        {
            if (id != titre.IdTitre)
            {
                return BadRequest();
            }
            return Ok(titre);
        }

    }
}
