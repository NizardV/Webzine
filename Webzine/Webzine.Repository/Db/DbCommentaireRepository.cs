using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Webzine.EntitiesContext;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Db
{
    /// <summary>
    /// Provides an implementation of the ICommentaireRepository interface that manages Commentaire entities using a
    /// WebzineDbContext for data access and an ILogger for logging operations.
    /// </summary>
    /// <remarks>This repository enables adding, deleting, and retrieving comments from the database. All
    /// operations are logged for traceability. Instances of this class are typically used in application layers that
    /// require persistent storage and retrieval of Commentaire entities. This class is not thread-safe; concurrent
    /// access should be managed externally if required.</remarks>
    public class DbCommentaireRepository : ICommentaireRepository
    {
        private readonly WebzineDbContext _context;
        private readonly ILogger<DbCommentaireRepository> _logger;

         /// <summary>
         /// Initialise une nouvelle instance de la classe DbCommentaireRepository avec le contexte de base de données
         /// et le journal spécifiés.
         /// </summary>
         /// <param name="context">Le contexte de base de données utilisé pour accéder aux entités de commentaires. Ne peut pas être null.</param>
         /// <param name="logger">Le journal utilisé pour enregistrer les informations de diagnostic et les erreurs. Ne peut pas être null.</param>
        public DbCommentaireRepository(WebzineDbContext context, ILogger<DbCommentaireRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        /// <summary>
        /// Adds a new comment to the data store and persists the change.
        /// </summary>
        /// <remarks>This method saves the new comment immediately to the underlying data store. Logging
        /// is performed before and after the operation. If the comment is successfully added, its identifier is updated
        /// with the generated value.</remarks>
        /// <param name="commentaire">The comment to add. Cannot be null. The comment's properties should be set appropriately before calling this
        /// method.</param>
        public void Add(Commentaire commentaire)
        {
            try
            {
                this._logger.LogDebug("Tentative d'ajout d'un nouveau commentaire (Auteur: {Auteur}, TitreID: {IdTitre}).", commentaire.Auteur, commentaire.IdTitre);

                this._context.Commentaires.Add(commentaire);
                this._context.SaveChanges();

                this._logger.LogDebug("Commentaire ajouté avec succès avec l'ID {IdCommentaire}.", commentaire.IdCommentaire);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de l'ajout du commentaire (Auteur: {Auteur}, TitreID: {IdTitre}).", commentaire.Auteur, commentaire.IdTitre);
                throw;
            }
        }

        /// <summary>
        /// Deletes the specified comment from the data store.
        /// </summary>
        /// <remarks>This method permanently removes the provided comment from the underlying database.
        /// Changes are immediately persisted. If the comment does not exist in the context, no action is
        /// taken.</remarks>
        /// <param name="commentaire">The comment to delete. Must not be null and must exist in the data context.</param>
        public void Delete(Commentaire commentaire)
        {
            try
            {
                this._logger.LogDebug("Tentative de suppression du commentaire ID {IdCommentaire}.", commentaire.IdCommentaire);

                this._context.Commentaires.Remove(commentaire);
                this._context.SaveChanges();

                this._logger.LogDebug("Commentaire ID {IdCommentaire} supprimé avec succès de la base de données.", commentaire.IdCommentaire);

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la suppression du commentaire ID {IdCommentaire}.", commentaire.IdCommentaire);
                throw;
            }
        }

        /// <summary>
        /// Recherche et retourne le commentaire correspondant à l'identifiant spécifié.
        /// </summary>
        /// <remarks>Le titre associé au commentaire est inclus dans le résultat. Cette méthode enregistre
        /// des informations dans les logs concernant la réussite ou l'échec de la recherche.</remarks>
        /// <param name="id">Identifiant unique du commentaire à rechercher.</param>
        /// <returns>Le commentaire correspondant à l'identifiant spécifié, ou null si aucun commentaire n'est trouvé.</returns>
        public Commentaire Find(int id)
        {

            try
            {
                this._logger.LogDebug("Recherche du commentaire avec l'ID {Id} dans la base de données...", id);

                var commentaire = this._context.Commentaires.Include(c => c.Titre).FirstOrDefault(c => c.IdCommentaire == id);

                if (commentaire == null)
                {
                    this._logger.LogWarning("Aucun commentaire trouvé avec l'ID {Id}.", id);
                }
                else
                {
                    this._logger.LogDebug("Commentaire ID {Id} récupéré avec succès.", id);
                }

                return commentaire;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche du commentaire avec l'ID {Id}.", id);
                throw;
            }
        }

        /// <summary>
        /// Récupère tous les commentaires disponibles dans la base de données.
        /// </summary>
        /// <remarks>Chaque élément retourné inclut également les informations associées à son titre via
        /// la propriété <c>Titre</c>.</remarks>
        /// <returns>Une collection contenant tous les objets <see cref="Commentaire"/> présents dans la base de données. La
        /// collection est vide s'il n'existe aucun commentaire.</returns>
        public IEnumerable<Commentaire> FindAll()
        {
            try
            {
                this._logger.LogDebug("Récupération de la liste complète des commentaires depuis la base de données...");

                var commentaires = this._context.Commentaires.Include(c => c.Titre).ToList();

                this._logger.LogDebug("{Count} commentaires récupérés avec succès.", commentaires.Count);

                return commentaires;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de tous les commentaires depuis la base de données.");
                throw;
            }
        }
    }
}