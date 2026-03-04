using Microsoft.AspNetCore.Server.Kestrel.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Webzine.Entity.Fixtures
{
    /// <summary>
    /// Factory permettant de créer des faux artistes.
    /// </summary>
    public class ArtisteFactory
    {
        /// <summary>
        /// Créer une liste d'artistes en dur.
        /// </summary>
        /// <return>Liste contenant des artistes</return>
        public static List<Artiste> CreerArtistes()
        {
            var artistes = new List<Artiste>();

            artistes.Add(new Artiste
            {
                Id = 1,
                Nom = "The Beatles",
                Biographie = "The Beatles était un groupe de rock britannique formé à Liverpool en 1960. Composé de John Lennon, Paul McCartney, George Harrison et Ringo Starr, le groupe est considéré comme l'un des plus influents de tous les temps. Leur musique a révolutionné la scène musicale et continue d'inspirer des générations d'artistes.",
            });

            artistes.Add(new Artiste
            {
                Id = 1,
                Nom = "The Beatles",
                Biographie = "The Beatles était un groupe de rock britannique formé à Liverpool en 1960. Composé de John Lennon, Paul McCartney, George Harrison et Ringo Starr, le groupe est considéré comme l'un des plus influents de tous les temps. Leur musique a révolutionné la scène musicale et continue d'inspirer des générations d'artistes.",
            });

            artistes.Add(new Artiste
            {
                Id = 1,
                Nom = "The Beatles",
                Biographie = "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest",
            });

            artistes.Add(new Artiste
            {
                Id = 1,
                Nom = "The Beatles",
                Biographie = "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest",
            });

            return artistes;
        }
    }
}
