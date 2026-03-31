namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Webzine.Entity;
    using Webzine.Business.Contracts.Services;
    using Webzine.Repository.Contracts;

    [Area("Administration")]
    [Route("administration")]
    /// <summary>
    /// Contrôleur d'administration des titres.
    /// Cette partie est orientée IHM : aucune persistance n'est effectuée.
    /// Les données proviennent des Factory et les modifications/créations sont simulées via TempData.
    /// </summary>
    public class TitreController : Controller
    {
        private readonly ITitreRepository _titreRepository;
        public TitreController(ILogger<TitreController> logger, ITitreRepository titreRepository)
        {
            _titreRepository = titreRepository;
        }

        [Route("titres")]
        public IActionResult Index()
        {
            var titres = _titreRepository.FindAll();
            return View(titres);
        }

        [HttpGet]
        [Route("titre/details/{id}")]
        public IActionResult Details(int id)
        {
            var titre = _titreRepository.Find(id);
            if (titre == null)
            {
                return NotFound();
            }

            return View(titre);
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
            if (!ModelState.IsValid)
            {
                return View(titre);
            }

            _titreRepository.Add(titre);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("titre/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var titre = _titreRepository.Find(id);
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

            if (!ModelState.IsValid)
            {
                return View(titre);
            }

            _titreRepository.Update(titre);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("titre/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var titre = _titreRepository.Find(id);
            if (titre == null)
            {
                return NotFound();
            }

            return View(titre);
        }

        [HttpPost]
        [Route("titre/delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var titre = _titreRepository.Find(id);
            if (titre == null)
            {
                return NotFound();
            }

            _titreRepository.Delete(titre);

            return RedirectToAction(nameof(Index));
        }
    }
}
