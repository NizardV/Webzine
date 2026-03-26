using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Db
{
    public class DbStyleRepository
    {
        private readonly DbContext _context;

        DbStyleRepository(DbContext context)
        {
            _context = context;
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
            _context.styles.Remove(style);
            _context.SaveChanges();
        }

        /// <summary>
        /// Implémentation de la méthode de recherche d'un style par son identifiant.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet style</returns>
        public Style Find(int id)
        {
            return _context.styles.FirstOrDefault(s => s.IdStyle == id);
        }

        /// <summary>
        /// Implémentation de la méthode de recherche de tous les styles.
        /// </summary>
        /// <returns>Une liste de styles</returns>
        public IEnumerable<Style> FindAll()
        {
            var styles = _context.styles;
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
