using System;
using System.Collections.Generic;
using System.Text;

namespace Webzine.Entity
{
    /// <summary>
    /// Regroupe les collections principales de données manipulées par le webzine.
    /// </summary>
    public class WebzineData
    {
        /// <summary>
        /// Obtient ou définit la collection des styles du webzine.
        /// </summary>
        public ICollection<Style> styles { get; set; } = new List<Style>();

        /// <summary>
        /// Obtient ou définit la collection des commentaires du webzine.
        /// </summary>
        public ICollection<Commentaire> commentaires { get; set; } = new List<Commentaire>();

        /// <summary>
        /// Obtient ou définit la collection des artistes du webzine.
        /// </summary>
        public ICollection<Artiste> artistes { get; set; } = new List<Artiste>();

        /// <summary>
        /// Obtient ou définit la collection des titres du webzine.
        /// </summary>
        public ICollection<Titre> titres { get; set; } = new List<Titre>();
    }
}
