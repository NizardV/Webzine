using Microsoft.AspNetCore.Mvc;
using Webzine.Entity;
using Webzine.Entity.Fixtures;
using Webzine.Repository.Contracts;
using Webzine.WebApplication.Areas.Administration.ViewModel.Commentaire;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
   /// <summary>
   /// Contrôleur ASP.NET Core MVC permettant de gérer les commentaires dans la zone d'administration de l'application.
   /// </summary>
   /// <remarks>Ce contrôleur fournit des actions pour afficher la liste des commentaires, afficher la
   /// confirmation de suppression et supprimer un commentaire. Toutes les actions sont accessibles via des routes
   /// spécifiques à la zone d'administration. L'accès à ce contrôleur peut nécessiter des autorisations
   /// d'administration selon la configuration de l'application.</remarks>
    [Area("Administration")]
    [Route("administration")]
    public class CommentaireController : Controller
    {

        private readonly ICommentaireRepository _commentaireRepository;

        /// <summary>
        /// Initialise une nouvelle instance de la classe CommentaireController avec le référentiel de commentaires
        /// spécifié.
        /// </summary>
        /// <param name="commentaireRepository">Le référentiel utilisé pour accéder et gérer les entités de commentaires. Ne peut pas être null.</param>
        public CommentaireController(ICommentaireRepository commentaireRepository)
        {
            this._commentaireRepository = commentaireRepository;
        }

        /// <summary>
        /// Affiche la liste des commentaires pour l'administration.
        /// </summary>
        /// <remarks>Utilisez cette action pour consulter et gérer tous les commentaires existants dans
        /// l'interface d'administration.</remarks>
        /// <returns>Un objet <see cref="IActionResult"/> qui rend la vue contenant les commentaires à des fins d'administration.</returns>
        [Route("commentaires")]
        public IActionResult Commentaires()
        {
            var commentaires = this._commentaireRepository.FindAll().ToList();
            return this.View(commentaires);
        }

        /// <summary>
        /// Affiche les détails d'un commentaire spécifique.
        /// </summary>
        /// <param name="id">L'identifiant unique du commentaire à afficher.</param>
        /// <returns>Un objet View affichant les détails du commentaire si celui-ci existe ; sinon, un résultat
        /// NotFound si aucun commentaire correspondant n'est trouvé.</returns>
        [HttpGet]
        [Route("commentaire/details/{id}")]
        public IActionResult Details(int id)
        {
            var commentaire = _commentaireRepository.Find(id);
            if (commentaire == null)
            {
                return NotFound();
            }

            return View(commentaire);
        }

        /// <summary>
        /// Affiche le formulaire de création d'un nouveau commentaire.
        /// </summary>
        /// <returns>Une vue contenant le formulaire de création d'un commentaire vide.</returns>
        [HttpGet]
        [Route("commentaire/create")]
        public IActionResult Create()
        {
            return View(new Commentaire());
        }

        /// <summary>
        /// Traite les données soumises par le formulaire de création d'un nouveau commentaire.
        /// </summary>
        /// <param name="commentaire">Le commentaire à créer.</param>
        /// <returns>Redirection vers la liste des commentaires si les données sont valides ; sinon retourne la vue avec les données saisies.</returns>
        [HttpPost]
        [Route("commentaire/create")]
        public IActionResult Create(Commentaire commentaire)
        {
            if (!ModelState.IsValid)
            {
                return View(commentaire);
            }

            _commentaireRepository.Add(commentaire);

            return RedirectToAction(nameof(Commentaires));
        }

        /// <summary>
        /// Affiche le formulaire d'édition d'un commentaire existant.
        /// </summary>
        /// <param name="id">L'identifiant unique du commentaire à modifier.</param>
        /// <returns>Une vue contenant le formulaire pré-rempli avec les données du commentaire, ou une réponse
        /// NotFound si le commentaire n'existe pas.</returns>
        [HttpGet]
        [Route("commentaire/edit/{id}")]
        public IActionResult Edit(int id)
        {
            var commentaire = _commentaireRepository.Find(id);
            if (commentaire == null)
            {
                return NotFound();
            }

            return View(commentaire);
        }

        /// <summary>
        /// Traite les données soumises par le formulaire d'édition d'un commentaire existant.
        /// </summary>
        /// <param name="id">L'identifiant unique du commentaire à modifier.</param>
        /// <param name="commentaire">Le commentaire contenant les données mises à jour.</param>
        /// <returns>Redirection vers la liste des commentaires si les données sont valides et l'ID correspond ; sinon retourne une réponse
        /// BadRequest ou la vue avec les données saisies.</returns>
        [HttpPost]
        [Route("commentaire/edit/{id}")]
        public IActionResult Edit(int id, Commentaire commentaire)
        {
            if (id != commentaire.IdCommentaire)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(commentaire);
            }

            _commentaireRepository.Update(commentaire);

            return RedirectToAction(nameof(Commentaires));
        }

        /// <summary>
        /// Affiche la vue de confirmation pour la suppression d'un commentaire spécifié par son identifiant.
        /// </summary>
        /// <remarks>Utilisez cette méthode pour afficher une page de confirmation avant de supprimer
        /// définitivement un commentaire. Cette action ne supprime pas le commentaire ; elle prépare uniquement la
        /// confirmation de la suppression.</remarks>
        /// <param name="id">L'identifiant unique du commentaire à supprimer.</param>
        /// <returns>Un objet View affichant la confirmation de suppression si le commentaire existe ; sinon, un résultat
        /// NotFound si aucun commentaire correspondant n'est trouvé.</returns>
        [HttpGet]
        [Route("commentaire/delete/{id}")]
        public IActionResult Supprimer(int id)
        {
            var commentaire = this._commentaireRepository.Find(id);
            if (commentaire is not null)
            {
                return this.View(commentaire);
            }
            else
            {
                return this.NotFound();
            }
        }

        /// <summary>
        /// Deletes the specified comment and redirects to the comment list if the comment exists.
        /// </summary>
        /// <param name="id">The identifier of the comment to delete.</param>
        /// <returns>A redirect to the comment list if the deletion is successful; otherwise, a NotFound result if the comment
        /// does not exist.</returns>
        [HttpPost]
        [Route("commentaire/delete/{id}")]
        public IActionResult DeleteConfirmed(int id)
        {

            var commentaire = this._commentaireRepository.Find(id);
            if (commentaire is not null)
            {
                this._commentaireRepository.Delete(commentaire);

                // On redirige vers l'action qui liste les commentaires
                return this.RedirectToAction(nameof(this.Commentaires));
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}
