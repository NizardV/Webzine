using Webzine.Entity;

namespace Webzine.Repository.Contracts
{
    /// <summary>
    /// Définit les opérations de persistance applicables aux artistes du webzine.
    /// </summary>
    public interface IArtisteRepository
    {
        /// <summary>
        /// Ajoute un nouvel artiste.
        /// </summary>
        /// <param name="artiste">Artiste à ajouter.</param>
        void Add(Artiste artiste);

        /// <summary>
        /// Retourne le nombre total d'artistes.
        /// </summary>
        /// <returns>Nombre total d'artistes.</returns>
        int Count();

        /// <summary>
        /// Supprime un artiste existant.
        /// </summary>
        /// <param name="artiste">Artiste à supprimer.</param>
        void Delete(Artiste artiste);

        /// <summary>
        /// Retourne un artiste à partir de son identifiant.
        /// </summary>
        /// <param name="idArtiste">Identifiant de l'artiste recherché.</param>
        /// <returns>L'artiste correspondant à l'identifiant fourni, ou null si aucun artiste n'est trouvé.</returns>
        Artiste? Find(int idArtiste);

        /// <summary>
        /// Retourne l'ensemble des artistes.
        /// </summary>
        /// <returns>Collection de tous les artistes.</returns>
        IEnumerable<Artiste> FindAll();

        /// <summary>
        /// Retourne un artiste à partir de son nom.
        /// </summary>
        /// <param name="nom">Nom de l'artiste recherché.</param>
        /// <returns>L'artiste correspondant au nom fourni, ou null si aucun artiste n'est trouvé.</returns>
        Artiste? FindByName(string nom);

        /// <summary>
        /// Met à jour un artiste existant.
        /// </summary>
        /// <param name="artiste">Artiste à mettre à jour.</param>
        void Update(Artiste artiste);
    }
}