using Webzine.Entity;
using Webzine.Entity.Fixtures;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    /// <summary>
    /// Provides an in-memory implementation of the ICommentaireRepository interface for managing Commentaire entities
    /// within the local application context.
    /// </summary>
    /// <remarks>This repository stores Commentaire instances in memory using the provided WebzineData
    /// context. It is suitable for scenarios such as testing or development where persistent storage is not required.
    /// Changes made through this repository affect only the in-memory collection and are not persisted beyond the
    /// application's lifetime.</remarks>
    public class LocalCommentaireRepository : ICommentaireRepository
    {

        private readonly WebzineData _data;

        /// <summary>
        /// Initialise une nouvelle instance de la classe LocalCommentaireRepository à l'aide de la source de données
        /// spécifiée.
        /// </summary>
        /// <param name="data">La source de données WebzineData utilisée pour accéder aux commentaires. Ne peut pas être null.</param>
        public LocalCommentaireRepository(WebzineData data)
        {
            _data = data;
        }

        /// <summary>
        /// Adds the specified comment to the collection of comments.
        /// </summary>
        /// <param name="commentaire">The comment to add to the collection. Cannot be null.</param>
        public void Add(Commentaire commentaire)
        {
            _data.commentaires.Add(commentaire);
        }

        /// <summary>
        /// Supprime le commentaire spécifié de la collection de commentaires si celui-ci existe.
        /// </summary>
        /// <remarks>Aucune exception n'est levée si le commentaire n'est pas trouvé dans la collection.
        /// Cette méthode ne modifie pas le paramètre passé en argument.</remarks>
        /// <param name="commentaire">Le commentaire à supprimer. Doit correspondre à un commentaire existant dans la collection, identifié par
        /// son identifiant unique.</param>
        public void Delete(Commentaire commentaire)
        {
            var elementASupprimer = _data.commentaires.FirstOrDefault(c => c.IdCommentaire == commentaire.IdCommentaire);

            if (elementASupprimer != null)
            {
                _data.commentaires.Remove(elementASupprimer);
            }
        }

        /// <summary>
        /// Recherche et retourne le commentaire correspondant à l'identifiant spécifié.
        /// </summary>
        /// <param name="id">L'identifiant unique du commentaire à rechercher.</param>
        /// <returns>Le commentaire correspondant à l'identifiant spécifié, ou null si aucun commentaire n'est trouvé.</returns>
        public Commentaire Find(int id)
        {
            return _data.commentaires.FirstOrDefault(c => c.IdCommentaire == id);
        }

       
        /// <summary>
        /// Returns a collection containing all available comments.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="Commentaire"/> objects representing all comments. The collection will
        /// be empty if no comments are available.</returns>
        public IEnumerable<Commentaire> FindAll()
        {
            return _data.commentaires;
        }
    }
}
