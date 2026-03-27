using Microsoft.AspNetCore.Mvc;
using Webzine.Entity;
using Webzine.Entity.Fixtures;
using Webzine.Repository.Contracts;
using Webzine.Repository.Db;
using Webzine.WebApplication.ViewModels;

namespace Webzine.WebApplication.Controllers
{
    /// <summary>
    /// Contrôleur gérant l'affichage des titres côté site public (détails, filtrage par style)
    /// ainsi que quelques interactions IHM sans persistance (like, édition légère).
    /// Les données restent issues des Factory.
    /// </summary>
    public class TitreController : Controller
    {
        private ITitreRepository _titreRepository;
        private ICommentaireRepository _commentaireRepository;
        public TitreController(ITitreRepository titreRepository, ICommentaireRepository commentaireRepository)
        {
            _titreRepository = titreRepository;
            _commentaireRepository = commentaireRepository;
        }

        [HttpPost]
        [Route("titre/like/{id}")]
        /// <summary>
        /// Incrémente le compteur de likes côté IHM.
        /// L'opération n'est pas persistée (Factory inchangées) et sert uniquement à refléter l'état dans l'interface.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        public IActionResult Like(int id)
        {
            var titre = _titreRepository.Find(id);
            if (titre is null)
            {
                return NotFound();
            }
            _titreRepository.IncrementNbLikes(titre);

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        [Route("Titre/Edit/{id}")]
        /// <summary>
        /// Affiche un formulaire d'édition légère (IHM) pour un titre.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        public IActionResult Edit(int id)
        {
            var titre = _titreRepository.Find(id);
            if (titre is null)
            {
                return NotFound();
            }
            return View(titre);
        }

        [HttpPost]
        [Route("Titre/Edit/{id}")]
        /// <summary>
        /// Enregistre l'édition légère (IHM) d'un titre, sans persistance.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        /// <param name="chronique">Nouvelle chronique à afficher.</param>
        public IActionResult Edit(int id, [FromForm] string chronique)
        {
            var titre = _titreRepository.Find(id);
            if (titre is null)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpGet]
        [Route("titres/style/{libelle}")]
        public IActionResult Style(string libelle)
        {
            // On utilise la méthode pour trouver le style correspondant au libellé de l'URL
            var titres = _titreRepository.SearchByStyle(libelle);

            if (titres == null)
            {
                return NotFound();
            }
            // On envoie la liste filtrée à la vue
            return View(titres);
        }

        /// <summary>
        /// Retourne la vue de détail d'un titre spécifique.
        /// </summary>
        /// <param name="id">Id du Titre</param>
        /// <returns>Une vue</returns>
        [Route("titre/{id}")]
        public IActionResult Details(int id)
        {
            Titre? titre = _titreRepository.Find(id);

            if (titre == null)
            {
                return this.NotFound();
            }

            var commentaires = _commentaireRepository.FindAll().Where(c => c.IdTitre == id).ToList();

            TitreModel model = new TitreModel
            {
                Titre = titre,
                Commentaires = commentaires,
            };

            return View(model);
        }
    }
}
