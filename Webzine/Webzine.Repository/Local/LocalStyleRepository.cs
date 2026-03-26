using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    public class LocalStyleRepository : IStyleRepository
    {
        /// <summary>
        /// Permet d'accéder à la collection de styles dans la classe WebzineData, 
        /// qui sert de source de données locale pour les opérations de gestion des styles.
        /// </summary>
        public readonly WebzineData _data;

        /// <summary>
        /// Constructeur de la classe LocalStyleRepository.
        /// </summary>
        /// <param name="data"></param>
        public LocalStyleRepository(WebzineData data)
        {
            _data = data;
        }

        /// <summary>
        /// Implémentation de la méthode d'ajout d'un style.
        /// </summary>
        /// <param name="style"></param>
        public void Add(Style style)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Implémentation de la méthode de suppression d'un style. 
        /// </summary>
        /// <param name="style"></param>
        public void Delete(Style style)
        {
            _data.styles.Remove(style);
        }

        /// <summary>
        /// Implémentation de la méthode de recherche d'un style par son identifiant.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet style</returns>
        public Style Find(int id)
        {
            return _data.styles.FirstOrDefault(s => s.IdStyle == id);
        }

        /// <summary>
        /// Implémentation de la méthode de recherche de tous les styles.
        /// </summary>
        /// <returns>Une liste de styles</returns>
        public IEnumerable<Style> FindAll()
        {
            var styles = _data.styles;
            return styles;
        }

        /// <summary>
        /// Implémentation de la méthode de mise à jour d'un style.
        /// </summary>
        /// <param name="style"></param>
        public void Update(Style style)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }
    }
}
