using Microsoft.Extensions.Logging;
using NLog;
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
        private readonly ILogger<LocalCommentaireRepository> _logger;

        /// <summary>
        /// Initialise une nouvelle instance de la classe LocalCommentaireRepository à l'aide de la source de données
        /// spécifiée.
        /// </summary>
        /// <param name="data">La source de données WebzineData utilisée pour accéder aux commentaires. Ne peut pas être null.</param>
        public LocalCommentaireRepository(WebzineData data, ILogger<LocalCommentaireRepository> logger)
        {
            this._logger = logger;
            this._data = data;
        }

        /// <summary>
        /// Adds the specified comment to the collection of comments.
        /// </summary>
        /// <param name="commentaire">The comment to add to the collection. Cannot be null.</param>
        public void Add(Commentaire commentaire)
        {
            try
            {
                this._logger.LogDebug("Tentative d'ajout d'un commentaire au repository local.");
                this._data.commentaires.Add(commentaire);
                this._logger.LogDebug("Commentaire ajouté avec succès au repository local.");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de l'ajout d'un commentaire au repository local.");
                throw new Exception("Impossible d'ajouter le commentaire au repository local.", ex);
            }
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
            this._logger.LogWarning(
                "Tentative de suppression du commentaire {IdCommentaire} du repository local. Opération non supportée sur les données en mémoire.",
                commentaire.IdCommentaire);
        }

        /// <summary>
        /// Recherche et retourne le commentaire correspondant à l'identifiant spécifié.
        /// </summary>
        /// <param name="id">L'identifiant unique du commentaire à rechercher.</param>
        /// <returns>Le commentaire correspondant à l'identifiant spécifié, ou null si aucun commentaire n'est trouvé.</returns>
        public Commentaire Find(int id)
        {
            try
            {
                this._logger.LogDebug(
                    "Recherche du commentaire d'identifiant {IdCommentaire} dans le repository local.",
                    id);

                var commentaire = this._data.commentaires.FirstOrDefault(c => c.IdCommentaire == id);

                if (commentaire != null)
                {
                    this._logger.LogDebug(
                        "Commentaire {IdCommentaire} trouvé.",
                        id);
                }
                else
                {
                    this._logger.LogDebug(
                        "Aucun commentaire trouvé dans le repository local pour l'identifiant {IdCommentaire}.",
                        id);
                }

                return commentaire;
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la recherche du commentaire {IdCommentaire} dans le repository local.",
                    id);
                throw new Exception($"Impossible de rechercher le commentaire avec l'ID {id} dans le repository local.", ex);
            }
        }


        /// <summary>
        /// Returns a collection containing all available comments.
        /// </summary>
        /// <returns>An enumerable collection of <see cref="Commentaire"/> objects representing all comments. The collection will
        /// be empty if no comments are available.</returns>
        public IEnumerable<Commentaire> FindAll()
        {
            try
            {
                this._logger.LogDebug(
                    "Récupération de tous les commentaires depuis le repository local.");

                return this._data.commentaires.ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la récupération de tous les commentaires depuis le repository local.");
                throw new Exception("Impossible de récupérer tous les commentaires du repository local.", ex);
            }
        }

        /// <summary>
        /// Updates an existing comment in the local collection.
        /// </summary>
        /// <remarks>This method updates the properties of an existing comment in memory. If the comment
        /// does not exist, no changes are made.</remarks>
        /// <param name="commentaire">The comment containing the updated data. Cannot be null.</param>
        public void Update(Commentaire commentaire)
        {
            try
            {
                this._logger.LogDebug(
                    "Tentative de mise à jour du commentaire {IdCommentaire} dans le repository local.",
                    commentaire.IdCommentaire);

                var existingCommentaire = _data.commentaires.FirstOrDefault(c => c.IdCommentaire == commentaire.IdCommentaire);
                if (existingCommentaire != null)
                {
                    existingCommentaire.Auteur = commentaire.Auteur;
                    existingCommentaire.Contenu = commentaire.Contenu;
                    existingCommentaire.IdTitre = commentaire.IdTitre;
                    existingCommentaire.DateCreation = commentaire.DateCreation;
                    this._logger.LogDebug(
                        "Commentaire {IdCommentaire} mis à jour avec succès dans le repository local.",
                        commentaire.IdCommentaire);
                }
                else
                {
                    this._logger.LogWarning(
                        "Commentaire {IdCommentaire} non trouvé dans le repository local.",
                        commentaire.IdCommentaire);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la mise à jour du commentaire {IdCommentaire} dans le repository local.",
                    commentaire.IdCommentaire);
                throw new Exception($"Impossible de mettre à jour le commentaire {commentaire.IdCommentaire} dans le repository local.", ex);
            }
        }
    }
}
