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
    public class LocalStylesFactory
    {

        private static readonly List<Style> styles = new Faker<Style>()
            .RuleFor(s => s.IdStyle, f => f.IndexFaker + 1)
            .RuleFor(s => s.Libelle, f => f.Music.Genre())
            .Generate(18)
            .DistinctBy(s => s.Libelle)
            .ToList();

        /// <summary>
        /// Crée une liste de styles de musique en dur.
        /// </summary>
        /// <returns>Liste contenant des styles de musique</returns>
        public static List<Style> CreerStyles()
        {
            return styles;
        }

        /// <summary>
        /// Récupère un style en fonction de son libellé (insensible à la casse).
        /// </summary>
        public static Style? GetStyleByLibelle(string libelle)
        {
            return CreerStyles().FirstOrDefault(s => s.Libelle.ToLower() == libelle.ToLower());
        }
    }

}