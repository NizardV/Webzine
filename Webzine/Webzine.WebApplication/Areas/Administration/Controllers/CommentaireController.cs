using Microsoft.AspNetCore.Mvc;
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
            _commentaireRepository = commentaireRepository;
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
            var commentaires = _commentaireRepository.FindAll().ToList();
            return View(new CommentairesAdministrationModel { Commentaires = commentaires });
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
            var commentaire = _commentaireRepository.Find(id);
            if (commentaire is not null)
            {
                return View(commentaire);
            }
            else
            {
                return NotFound();
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

            var commentaire = _commentaireRepository.Find(id);
            if (commentaire is not null)
            {
                _commentaireRepository.Delete(commentaire);

                // On redirige vers l'action qui liste les commentaires
                return RedirectToAction(nameof(Commentaires));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
