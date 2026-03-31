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
        private readonly ILogger<StyleController> _logger;

        public StyleController(IStyleRepository styleRepository, ILogger<StyleController> logger)
        {
            _styleRepository = styleRepository;
            _logger = logger;
        }


        /// <summary>
        /// Retourne une vue qui affiche la liste de tous les styles de musique présents dans la collection de styles.
        /// </summary>
        /// <returns>Une vue.</returns>
        [Route("styles")]
        public IActionResult Index()
        {
            try
            {
                this._logger.LogInformation("Consultation de la liste des styles.");
                var styles = _styleRepository.FindAll().ToList();
                return View(styles);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de la liste des styles.");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retourne une vue qui affiche les détails d'un style de musique.
        /// </summary>
        /// <param name="id">L'identifiant du style de musique à afficher.</param>
        /// <returns>Une vue contenant le style, ou une réponse NotFound si le style n'existe pas.</returns>
        [HttpGet]
        [Route("style/details/{id}")]
        public IActionResult Details(int id)
        {
            try
            {
                this._logger.LogInformation("Demande d'affichage des détails du style {IdStyle}.", id);

                var style = _styleRepository.Find(id);

                if (style == null)
                {
                    this._logger.LogWarning("Aucun style trouvé avec l'id {IdStyle}.", id);
                    return NotFound();
                }

                return View(style);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du chargement des détails du style {IdStyle}.", id);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Returne une vue pour créer un nouveau style.
        /// </summary>
        /// <returns>Une vue.</returns>
        [HttpGet]
        [Route("style/create")]
        public IActionResult Create()
        {
            this._logger.LogInformation("Affichage du formulaire de création d'un style.");
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
            try
            {
                if (!ModelState.IsValid)
                {
                    this._logger.LogWarning("Échec de création d'un style : modèle invalide.");

                    return View(style);
                }

                _styleRepository.Add(style);

                this._logger.LogInformation("Création du style réussie. Libellé : {Libelle}.", style.Libelle);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la création du style {Libelle}.", style.Libelle);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Retourne une vue permettant d'afficher le formulaire de modification d'un style de musique existant.
        /// </summary>
        /// <param name="id">L'identifiant du style de musique à modifier.</param>
        /// <returns>Une vue contenant le style à modifier, ou une réponse NotFound si le style n'existe pas.</returns>
        [HttpGet]
        [Route("style/edit/{id}")]
        public IActionResult Edit(int id) {
            try
            {
                this._logger.LogInformation("Demande d'affichage du formulaire de modification du style {IdStyle}.", id);

                var style = _styleRepository.Find(id);

                if (style == null)
                {
                    this._logger.LogWarning("Aucun style trouvé pour l'id {IdStyle}.", id);
                    return NotFound();
                }

                return View(style);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du chargement du style {IdStyle}.", id);
                return StatusCode(500);
            }
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
            try
            {
                if (id != style.IdStyle)
                {
                    this._logger.LogWarning(
                            "Échec de modification : incohérence entre l'id de route {RouteId} et l'id du style {StyleId}.",
                            id,
                            style.IdStyle);
                    return BadRequest();
                }

                if (!ModelState.IsValid)
                {
                    this._logger.LogWarning("Échec de modification du style {IdStyle} : modèle invalide.", id);
                    return View(style);
                }

                var styleExistant = _styleRepository.Find(id);

                if (styleExistant == null)
                {
                    this._logger.LogWarning("Échec de modification : aucun style trouvé pour l'id {IdStyle}.", id);
                    return NotFound();
                }

                _styleRepository.Update(style);

                this._logger.LogInformation("Modification du style {IdStyle} réussie.", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la modification du style {IdStyle}.", id);
                return StatusCode(500);
            }
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
            try
            {
                this._logger.LogInformation("Demande d'affichage de la confirmation de suppression du style {IdStyle}.", id);

                var style = _styleRepository.Find(id);

                if (style == null)
                {
                    this._logger.LogWarning("Aucun style trouvé pour suppression avec l'id {IdStyle}.", id);
                    return NotFound();
                }

                return View(style);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du chargement du style {IdStyle} pour suppression.", id);
                return StatusCode(500);
            }
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
            try
            {
                var style = _styleRepository.Find(id);

                if (style == null)
                {
                    this._logger.LogWarning("Échec de suppression : aucun style trouvé pour l'id {IdStyle}.", id);
                    return NotFound();
                }

                _styleRepository.Delete(style);

                this._logger.LogInformation("Suppression du style {IdStyle} réussie.", id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la suppression du style {IdStyle}.", id);
                return StatusCode(500);
            }
        }
    }
}