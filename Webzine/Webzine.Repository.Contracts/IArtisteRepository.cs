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

        /// <summary>
        /// Obtient le nombre total de biographies d'artistes disponibles.
        /// </summary>
        /// <returns>Le nombre de biographies d'artistes actuellement enregistrées. La valeur retournée est toujours supérieure
        /// ou égale à zéro.</returns>
        int CountWithBiography();

        /// <summary>
        /// Obtient l'artiste ayant le plus grand nombre de chroniques.
        /// </summary>
        /// <returns>L'instance de l'artiste avec le plus de chroniques. Retourne null si aucun artiste n'est disponible.</returns>
        Artiste? GetArtistePlusChronique();

        /// <summary>
        /// Obtient l'artiste ayant le plus grand nombre d'albums.
        /// </summary>
        /// <returns>L'instance de l'artiste avec le plus d'albums. Retourne null si aucun artiste n'est disponible.</returns>
        Artiste? GetArtistePlusAlbums();
    }
}