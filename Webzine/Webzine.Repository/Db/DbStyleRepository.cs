using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;

namespace Webzine.Repository.Db
{
    public class DbStyleRepository
    {
        /// <summary>
        /// Permet d'accéder à la base de données via une instance de DbContext.
        /// </summary>
        private readonly DbContext _context;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="DbStyleRepository"/> class.
        /// Constructeur de la classe DbStyleRepository qui prend en paramètre une instance de DbContext.
        /// </summary>
        /// <param name="context"></param>
        DbStyleRepository(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Ajoute un nouveau style de musique à la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        public void Add(Style style)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Supprime un style de musique de la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        public void Delete(Style style)
        {
            _context.styles.Remove(style);
            _context.SaveChanges();
        }

        /// <summary>
        /// Retourne un style de musique dans la collection de styles en utilisant son identifiant unique.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet style</returns>
        public Style Find(int id)
        {
            return _context.styles.FirstOrDefault(s => s.IdStyle == id);
        }

        /// <summary>
        /// Retourne une liste de tous les styles de musique présents dans la collection de styles.
        /// </summary>
        /// <returns>Une liste de styles</returns>
        public IEnumerable<Style> FindAll()
        {
            var styles = _context.styles;
            return styles;
        }

        /// <summary>
        /// Met à jour les informations d'un style de musique existant dans la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        public void Update(Style style)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }
    }
}
