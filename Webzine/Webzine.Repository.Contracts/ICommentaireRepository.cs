using System;
using System.Collections.Generic;
using Webzine.Entity;

namespace Webzine.Repository.Contracts
{
    /// <summary>
    /// Définit les opérations de persistance applicables aux commentaires du webzine.
    /// </summary>
    public interface ICommentaireRepository
    {
        /// <summary>
        /// Ajoute un nouveau commentaire.
        /// </summary>
        /// <param name="commentaire">Commentaire à ajouter.</param>
        void Add(Commentaire commentaire);

        /// <summary>
        /// Supprime un commentaire existant.
        /// </summary>
        /// <param name="commentaire">Commentaire à supprimer.</param>
        void Delete(Commentaire commentaire);

        /// <summary>
        /// Retourne un commentaire à partir de son identifiant.
        /// </summary>
        /// <param name="id">Identifiant du commentaire recherché.</param>
        /// <returns>Le commentaire correspondant à l'identifiant fourni.</returns>
        Commentaire Find(int id);

        /// <summary>
        /// Retourne l'ensemble des commentaires.
        /// </summary>
        /// <returns>Collection de tous les commentaires.</returns>
        IEnumerable<Commentaire> FindAll();
        void Update(Commentaire commentaire);
    }
}