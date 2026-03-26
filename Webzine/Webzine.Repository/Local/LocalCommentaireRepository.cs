using Webzine.Entity;
using Webzine.Entity.Fixtures;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    /// <summary>
    /// Provides an in-memory implementation of the ICommentaireRepository interface for managing Commentaire entities.
    /// </summary>
    /// <remarks>This repository simulates data storage using a local collection and is intended for scenarios
    /// such as testing or prototyping where persistent storage is not required. Changes made through this repository do
    /// not persist beyond the lifetime of the collection instance.</remarks>
    public class LocalCommentaireRepository : ICommentaireRepository
    {
        /// <summary>
        /// Adds a new comment to the collection, assigning it a unique identifier.
        /// </summary>
        /// <remarks>The method simulates database auto-increment behavior by assigning a new identifier
        /// to the comment before adding it. The comment's identifier will be set to one greater than the current
        /// maximum identifier in the collection.</remarks>
        /// <param name="commentaire">The comment to add to the collection. Cannot be null.</param>
        public void Add(Commentaire commentaire)
        {
            var commentaires = CommentaireFactory.CreerCommentaires();
            // On simule l'auto-incrémentation de la base de données
            commentaire.IdCommentaire = commentaires.Max(c => c.IdCommentaire) + 1;

            commentaires.Add(commentaire);
        }

        /// <summary>
        /// Supprime le commentaire spécifié de la collection de commentaires.
        /// </summary>
        /// <remarks>Si le commentaire spécifié n'existe pas dans la collection, aucune action n'est
        /// effectuée.</remarks>
        /// <param name="commentaire">Le commentaire à supprimer. Doit contenir un identifiant valide correspondant à un commentaire existant dans
        /// la collection.</param>
        public void Delete(Commentaire commentaire)
        {
            var commentaires = CommentaireFactory.CreerCommentaires();
            var elementASupprimer = commentaires.FirstOrDefault(c => c.IdCommentaire == commentaire.IdCommentaire);

            if (elementASupprimer != null)
            {
                commentaires.Remove(elementASupprimer);
            }
        }

        /// <summary>
        /// Recherche et retourne le commentaire correspondant à l'identifiant spécifié.
        /// </summary>
        /// <param name="id">L'identifiant unique du commentaire à rechercher.</param>
        /// <returns>Le commentaire dont l'identifiant correspond à la valeur de <paramref name="id"/> ; ou <see
        /// langword="null"/> si aucun commentaire correspondant n'est trouvé.</returns>
        public Commentaire Find(int id)
        {
            var commentaires = CommentaireFactory.CreerCommentaires();
            return commentaires.FirstOrDefault(c => c.IdCommentaire == id);
        }

        /// <summary>
        /// Récupère tous les objets de type Commentaire disponibles.
        /// </summary>
        /// <returns>Une collection d'objets Commentaire représentant l'ensemble des commentaires disponibles. La collection peut
        /// être vide si aucun commentaire n'est disponible.</returns>
        public IEnumerable<Commentaire> FindAll()
        {
            return CommentaireFactory.CreerCommentaires();
        }
    }
}
