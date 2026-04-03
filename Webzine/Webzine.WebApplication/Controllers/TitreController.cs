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

        /// <summary>
        /// Initializes a new instance of the TitreController class with the specified repositories.
        /// </summary>
        /// <param name="titreRepository">The repository used to access and manage titre entities. Cannot be null.</param>
        /// <param name="commentaireRepository">The repository used to access and manage commentaire entities. Cannot be null.</param>
        public TitreController(ITitreRepository titreRepository, ICommentaireRepository commentaireRepository)
        {
            this._titreRepository = titreRepository;
            this._commentaireRepository = commentaireRepository;
        }

        [HttpPost]
        /// <summary>
        /// Incrémente le compteur de likes côté IHM.
        /// L'opération n'est pas persistée (Factory inchangées) et sert uniquement à refléter l'état dans l'interface.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        public IActionResult Like(int id)
        {
            var titre = this._titreRepository.Find(id);
            if (titre is null)
            {
                return this.NotFound();
            }
            this._titreRepository.IncrementNbLikes(titre);

            return this.RedirectToAction(nameof(this.Details), new { id });
        }

        /// <summary>
        /// Affiche un formulaire d'édition légère (IHM) pour un titre.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        public IActionResult Edit(int id)
        {
            var titre = this._titreRepository.Find(id);
            if (titre is null)
            {
                return this.NotFound();
            }

            return this.View(titre);
        }

        [HttpPost]
        /// <summary>
        /// Enregistre l'édition légère (IHM) d'un titre, sans persistance.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        /// <param name="chronique">Nouvelle chronique à afficher.</param>
        public IActionResult Edit(int id, [FromForm] string chronique)
        {
            var titre = this._titreRepository.Find(id);
            if (titre is null)
            {
                return this.NotFound();
            }
            titre.Chronique = chronique;
            this._titreRepository.Update(titre);
            TempData["nouvelleLecture"] = true;
            return this.RedirectToAction(nameof(this.Details), new { id });
        }

        public IActionResult Style(string libelle)
        {
            // On utilise la méthode pour trouver le style correspondant au libellé de l'URL
            var titres = this._titreRepository.SearchByStyle(libelle);

            var model = new TitresParStyleViewModel
            {
                NomDuStyle = libelle,
                Titres = titres,
            };

            // On envoie la liste filtrée à la vue
            return this.View(model);
        }

        /// <summary>
        /// Retourne la vue de détail d'un titre spécifique.
        /// </summary>
        /// <param name="id">Id du Titre</param>
        /// <returns>Une vue</returns>
        public IActionResult Details(int id)
        {
            Titre titre = this._titreRepository.Find(id);

            if (titre == null)
            {
                return this.NotFound();
            }

            if (TempData["nouvelleLecture"] is null)
            {
                this._titreRepository.IncrementNbLectures(titre);
            }

            var model = new TitreModel { Titre = titre };

            return this.View(model);
        }


        /// <summary>
        /// Permet d'ajouter un commentaire à un titre spécifique. Le commentaire est créé à partir des données du formulaire soumis, puis ajouté à la base de données via le CommentaireRepository. Après l'ajout, l'utilisateur est redirigé vers la page de détails du titre pour voir le nouveau commentaire.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Commenter(int id, TitreModel viewModel)
        {
            ModelState.Remove("Titre");

            if (!ModelState.IsValid)
            {
                Titre titre = this._titreRepository.Find(id);
                return this.View("Details", viewModel);
            }
            var titreComm = this._titreRepository.Find(id);
            var nouveauCommentaire = new Commentaire
            {
                IdTitre = id,
                Auteur = viewModel.Auteur,
                Contenu = viewModel.Contenu,
                DateCreation = DateTime.Now,
                Titre = titreComm,
            };
            this._commentaireRepository.Add(nouveauCommentaire);
            titreComm.Commentaires.Add(nouveauCommentaire);
            this._titreRepository.Update(titreComm);
            TempData["nouvelleLecture"] = true;
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
