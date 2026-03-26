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

        /// <summary>
        /// Retourne une vue qui affiche la liste de tous les styles de musique présents dans la collection de styles.
        /// </summary>
        /// <returns>Une vue.</returns>
        public IActionResult Index()
        {
            var styles = _styleRepository.FindAll();
            return View(styles);
        }

        /// <summary>
        /// Returne une vue pour créer un nouveau style.
        /// </summary>
        /// <returns>Une vue.</returns>
        [HttpGet]
        [Route("style/create")]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Traite les données soumises par le formulaire de création d'un nouveau style de musique.
        /// </summary>
        /// <param name="style"></param>
        /// <returns>Redirection vers l'affichage de tous les styles</returns>
        [HttpPost]
        [Route("style/create")]
        public IActionResult Create(Style style)
        {
            if (!ModelState.IsValid)
            {
                return View(style);
            }

            _styleRepository.Add(style);

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Retourne une vue permettant d'afficher le formulaire de modification d'un style de musique existant.
        /// </summary>
        /// <param name="id">L'identifiant du style de musique à modifier.</param>
        /// <returns>Une vue contenant le style à modifier, ou une réponse NotFound si le style n'existe pas.</returns>
        [HttpGet]
        [Route("style/edit/{id}")]
        public IActionResult Edit(int id) {
            var style = _styleRepository.Find(id);

            if (style == null) { 
                return NotFound();
            }

            return View(style);
        }

        /// <summary>
        /// Traite les données soumises par le formulaire de modification d'un style de musique existant.
        /// </summary>
        /// <param name="id">L'identifiant du style de musique à modifier.</param>
        /// <param name="style">Le style de musique contenant les nouvelles données.</param>
        /// <returns>Redirection vers l'affichage de tous les styles si les données sont valides ; sinon retourne la vue avec les données saisies.</returns>
        [HttpPost]
        [Route("style/edit/{id}")]
        public IActionResult Edit(int id, Style style)
        {
            if (id != style.IdStyle)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(style);
            }

            var styleExistant = _styleRepository.Find(id);

            if (styleExistant == null)
            {
                return NotFound();
            }

            _styleRepository.Update(style);
            
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Retourne une vue permettant d'afficher la confirmation de suppression d'un style de musique.
        /// </summary>
        /// <param name="id">L'identifiant du style de musique à supprimer.</param>
        /// <returns>Une vue contenant le style à supprimer, ou une réponse NotFound si le style n'existe pas.</returns>
        [HttpGet]
        [Route("style/delete/{id}")]
        public IActionResult Delete(int id)
        {
            var style = _styleRepository.Find(id);

            if (style == null)
            {
                return NotFound();
            }

            return View(style);
        }

        /// <summary>
        /// Traite la confirmation de suppression d'un style de musique existant.
        /// </summary>
        /// <param name="id">L'identifiant du style de musique à supprimer.</param>
        /// <returns>Redirection vers l'affichage de tous les styles si la suppression est effectuée ; sinon retourne une réponse NotFound.</returns>
        [HttpPost]
        [Route("style/delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {
            var titre = _styleRepository.Find(id);

            if (titre == null) {
                return NotFound();
            }

            _styleRepository.Delete(titre);

            return RedirectToAction(nameof(Index));
        }
    }
}