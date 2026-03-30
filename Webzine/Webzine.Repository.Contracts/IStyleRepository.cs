using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Webzine.Entity;

namespace Webzine.Repository.Contracts
{
    /// <summary>
    /// Définit les opérations de persistance applicables aux styles du webzine.
    /// </summary>
    public interface IStyleRepository
    {
        /// <summary>
        /// Ajoute un nouveau style de musique à la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        void Add(Style style);

        /// <summary>
        /// Supprime un style de musique de la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        void Delete(Style style);

        /// <summary>
        /// Retourne un style de musique dans la collection de styles en utilisant son identifiant unique.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet style</returns>
        Style Find(int id);

        /// <summary>
        /// Retourne une liste de tous les styles de musique présents dans la collection de styles.
        /// </summary>
        /// <returns>Une liste de styles</returns>
        IEnumerable<Style> FindAll();

        /// <summary>
        /// Met à jour les informations d'un style de musique existant dans la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        void Update(Style style);
    }
}
