namespace Webzine.Entity.Fixtures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Webzine.Entity;

    /// <summary>
    /// Factory permettant de créer des faux styles de musique (en dur).
    /// </summary>
    public class LocalStylesFactory
    {
        /// <summary>
        /// Crée une liste de styles de musique en dur.
        /// </summary>
        /// <returns>Liste contenant des styles de musique en dur</returns>
        public static List<Style> CreerStyles()
        {
            var styles = new List<Style>();

            styles.Add(new Style { Id = 1, Libelle = "Acid house" });
            styles.Add(new Style { Id = 2, Libelle = "Ambient" });
            styles.Add(new Style { Id = 3, Libelle = "Cosmic" });
            styles.Add(new Style { Id = 4, Libelle = "Deep house" });
            styles.Add(new Style { Id = 5, Libelle = "Disco" });
            styles.Add(new Style { Id = 6, Libelle = "Downtempo" });
            styles.Add(new Style { Id = 7, Libelle = "Drum n bass" });
            styles.Add(new Style { Id = 8, Libelle = "Dub techno" });
            styles.Add(new Style { Id = 9, Libelle = "Electro" });

            return styles;
        }
    }

}