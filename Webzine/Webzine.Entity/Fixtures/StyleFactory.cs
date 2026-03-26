namespace Webzine.Entity.Fixtures
{
    using Bogus;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Webzine.Entity;

    /// <summary>
    /// Factory permettant de créer des faux styles de musique avec Bogus.
    /// </summary>
    public class StyleFactory
    {
        /// <summary>
        /// Crée une liste de styles de musique en dur.
        /// </summary>
        /// <returns>Liste contenant des styles de musique</returns>
        public static List<Style> CreerStyles(int nombreDeStyles)
        {
            var styles = new Faker<Style>()
                .RuleFor(s => s.IdStyle, f => f.IndexFaker + 1)
                .RuleFor(s => s.Libelle, f => f.Music.Genre())
                .Generate(nombreDeStyles)
                .DistinctBy(s => s.Libelle)
                .ToList();

            return styles;
        }
    }
}