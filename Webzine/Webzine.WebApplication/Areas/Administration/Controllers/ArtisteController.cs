using Microsoft.AspNetCore.Mvc;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("administration")]
    public class ArtisteController : Controller
    {
        private readonly IArtisteRepository artisteRepository;

        public ArtisteController(IArtisteRepository artisteRepository)
        {
            this.artisteRepository = artisteRepository;
        }

        [Route("artistes")]
        public IActionResult Index()
        {
            var artistes = this.artisteRepository.FindAll();
            return View(artistes);
        }

        [HttpGet]
        [Route("artiste/details/{id}")]
        public IActionResult Details(int id)
        {
            var artiste = this.artisteRepository.Find(id);
            if (artiste == null)
            {
                return NotFound();
            }

            return View(artiste);
        }

        [HttpGet]
        [Route("artiste/create")]
        public IActionResult Create()
        {
            return View(new Artiste());
        }

        [HttpPost]
        [Route("artiste/create")]
        public IActionResult Create(Artiste artiste)
        {
            if (!ModelState.IsValid)
            {
                return View(artiste);
            }

            this.artisteRepository.Add(artiste);
            return RedirectToAction(nameof(Index));
        }

        // Affiche le formulaire d'édition pré-rempli
        [HttpGet]
        [Route("artiste/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var artiste = this.artisteRepository.Find(id);

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

            if (!ModelState.IsValid)
            {
                return View(artiste);
            }

            this.artisteRepository.Update(artiste);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Route("artiste/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var artiste = this.artisteRepository.Find(id);

            if (artiste == null)
            {
                return NotFound();
            }

            return View(artiste);
        }

        [HttpPost]
        [Route("artiste/delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var artiste = this.artisteRepository.Find(id);
            if (artiste != null)
            {
                this.artisteRepository.Delete(artiste);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
