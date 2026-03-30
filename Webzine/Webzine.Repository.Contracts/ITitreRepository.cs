using Webzine.Entity;

namespace Webzine.Repository.Contracts
{
    /// <summary>
    /// Définit les opérations de persistance applicables aux titres du webzine.
    /// </summary>
    public interface ITitreRepository
    {
        /// <summary>
        /// Ajoute un nouveau titre.
        /// </summary>
        /// <param name="titre">Titre à ajouter.</param>
        void Add(Titre titre);

        /// <summary>
        /// Retourne le nombre total de titres.
        /// </summary>
        /// <returns>Nombre total de titres.</returns>
        int Count();

        /// <summary>
        /// Supprime un titre existant.
        /// </summary>
        /// <param name="titre">Titre à supprimer.</param>
        void Delete(Titre titre);

        /// <summary>
        /// Retourne un titre à partir de son identifiant.
        /// </summary>
        /// <param name="idTitre">Identifiant du titre recherché.</param>
        /// <returns>Le titre correspondant à l'identifiant fourni.</returns>
        Titre Find(int idTitre);

        /// <summary>
        /// Retourne une liste paginée de titres,
        /// triés du plus récent au plus ancien selon leur date de création.
        /// </summary>
        /// <param name="offset">Position de départ dans la liste des titres.</param>
        /// <param name="limit">Nombre maximal de titres à retourner.</param>
        /// <returns>Collection paginée de titres.</returns>
        IEnumerable<Titre> FindTitres(int offset, int limit);

        /// <summary>
        /// Retourne l'ensemble des titres.
        /// </summary>
        /// <returns>Collection de tous les titres.</returns>
        IEnumerable<Titre> FindAll();

        /// <summary>
        /// Incrémente de 1 le nombre de lectures d'un titre.
        /// </summary>
        /// <param name="titre">Titre concerné.</param>
        void IncrementNbLectures(Titre titre);

        /// <summary>
        /// Incrémente de 1 le nombre de likes d'un titre.
        /// </summary>
        /// <param name="titre">Titre concerné.</param>
        void IncrementNbLikes(Titre titre);

        /// <summary>
        /// Recherche les titres contenant le mot fourni,
        /// de manière insensible à la casse.
        /// </summary>
        /// <param name="mot">Mot recherché.</param>
        /// <returns>Collection de titres correspondant à la recherche.</returns>
        IEnumerable<Titre> Search(string mot);

        /// <summary>
        /// Recherche les titres associés à un style de musique donné,
        /// de manière insensible à la casse.
        /// </summary>
        /// <param name="libelle">Libellé du style recherché.</param>
        /// <returns>Collection de titres correspondant au style recherché.</returns>
        IEnumerable<Titre> SearchByStyle(string libelle);

        /// <summary>
        /// Met à jour un titre existant.
        /// </summary>
        /// <param name="titre">Titre à mettre à jour.</param>
        void Update(Titre titre);
    }
}