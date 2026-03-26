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
        /// Permet d'accéder à la liste des styles en mémoire. 
        /// Cette liste est utilisée pour stocker les styles de manière temporaire pendant l'exécution de l'application.
        /// </summary>
        public readonly WebzineData _data;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="LocalStyleRepository"/> class.
        /// Constructeur de la classe LocalStyleRepository qui prend en paramètre une instance de WebzineData.
        /// </summary>
        /// <param name="data"></param>
        public LocalStyleRepository(WebzineData data)
        {
            _data = data;
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
            _data.styles.Remove(style);
        }

        /// <summary>
        /// Retourne un style de musique dans la collection de styles en utilisant son identifiant unique.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet style</returns>
        public Style Find(int id)
        {
            return _data.styles.FirstOrDefault(s => s.IdStyle == id);
        }

        /// <summary>
        /// Retourne une liste de tous les styles de musique présents dans la collection de styles.
        /// </summary>
        /// <returns>Une liste de styles</returns>
        public IEnumerable<Style> FindAll()
        {
            var styles = _data.styles;
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
