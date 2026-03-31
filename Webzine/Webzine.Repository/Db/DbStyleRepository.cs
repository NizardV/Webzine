using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Webzine.EntitiesContext;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Db
{
    /// <inheritdoc/>
    public class DbStyleRepository : IStyleRepository
    {
        /// <summary>
        /// Permet d'accéder à la base de données via une instance de DbContext.
        /// </summary>
        private readonly WebzineDbContext _context;
        private readonly ILogger<DbStyleRepository> _logger;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="DbStyleRepository"/> class.
        /// Constructeur de la classe DbStyleRepository qui prend en paramètre une instance de DbContext.
        /// </summary>
        /// <param name="context"></param>
        public DbStyleRepository(WebzineDbContext context, ILogger<DbStyleRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        /// <summary>
        /// Ajoute un nouveau style de musique à la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        public void Add(Style style)
        {
            this._logger.LogWarning(
                "Tentative d'ajout du style {Libelle}. Méthode non encore implémentée dans le repository base de données.",
                style.Libelle);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Supprime un style de musique de la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        public void Delete(Style style)
        {
            try
            {
                this._logger.LogDebug(
                    "Suppression du style {IdStyle} demandée dans le repository base de données.",
                    style.IdStyle);

                this._context.Styles.Remove(style);
                this._context.SaveChanges();

                this._logger.LogDebug(
                    "Suppression du style {IdStyle} réussie dans le repository base de données.",
                    style.IdStyle);
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la suppression du style {IdStyle} dans le repository base de données.",
                    style.IdStyle);
                throw;
            }
        }

        /// <summary>
        /// Retourne un style de musique dans la collection de styles en utilisant son identifiant unique.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet style</returns>
        public Style Find(int id)
        {
            try
            {
                this._logger.LogDebug(
                    "Recherche du style {IdStyle} dans le repository base de données.",
                    id);

                var style = this._context.Styles.FirstOrDefault(s => s.IdStyle == id);

                if (style == null)
                {
                    this._logger.LogWarning(
                        "Aucun style trouvé pour l'identifiant {IdStyle} dans le repository base de données.",
                        id);
                }

                return style;
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la recherche du style {IdStyle} dans le repository base de données.",
                    id);
                throw;
            }
        }

        /// <summary>
        /// Retourne une liste de tous les styles de musique présents dans la collection de styles.
        /// </summary>
        /// <returns>Une liste de styles</returns>
        public IEnumerable<Style> FindAll()
        {
            try
            {
                this._logger.LogDebug(
                    "Récupération de tous les styles depuis le repository base de données.");

                return this._context.Styles.ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la récupération de tous les styles depuis le repository base de données.");
                throw;
            }
        }

        /// <summary>
        /// Met à jour les informations d'un style de musique existant dans la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        public void Update(Style style)
        {
            this._logger.LogWarning(
                "Tentative de modification du style {IdStyle}. Méthode non encore implémentée dans le repository base de données.",
                style.IdStyle);

            throw new NotImplementedException();
        }

        /// </inheritdoc/>
        public int Count()
        {
            try
            {
                return this._context.Styles.Count();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du comptage des styles en base de données.");
                throw;
            }
        }
    }
}
