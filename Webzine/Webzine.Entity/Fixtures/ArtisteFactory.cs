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
        /// Récupérer tous les artistes.
        /// </summary>
        /// <return>Liste contenant des artistes</return>
        public static List<Artiste> TousLesArtistes()
        {
            var artistes = new List<Artiste>();

            artistes.Add(new Artiste
            {
                IdArtiste = 1,
                Nom = "The Beatles",
                Biographie = "The Beatles était un groupe de rock britannique formé à Liverpool en 1960. Composé de John Lennon, Paul McCartney, George Harrison et Ringo Starr, le groupe est considéré comme l'un des plus influents de tous les temps. Leur musique a révolutionné la scène musicale et continue d'inspirer des générations d'artistes.",
            });

            artistes.Add(new Artiste
            {
                IdArtiste = 1,
                Nom = "The Beatles",
                Biographie = "The Beatles était un groupe de rock britannique formé à Liverpool en 1960. Composé de John Lennon, Paul McCartney, George Harrison et Ringo Starr, le groupe est considéré comme l'un des plus influents de tous les temps. Leur musique a révolutionné la scène musicale et continue d'inspirer des générations d'artistes.",
            });

            artistes.Add(new Artiste
            {
                IdArtiste = 1,
                Nom = "The Beatles",
                Biographie = "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest",
            });

            artistes.Add(new Artiste
            {
                IdArtiste = 1,
                Nom = "The Beatles",
                Biographie = "testtesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttesttest",
            });

            return artistes;
        }

        /// <summary>
        /// Récupérer un artiste par son id
        /// </summary>
        /// <return>Un object artiste</return>
        public static Artiste? TrouverParId(int artisteId)
        {
            return TousLesArtistes().FirstOrDefault(a => a.IdArtiste == artisteId);
        }

        /// <summary>
        /// Récupérer un artiste par son nom.
        /// </summary>
        /// <return>Un object artiste</return>
        public static Artiste? TrouverParNom(string nom)
        {
            if (string.IsNullOrWhiteSpace(nom)) // Vérifier si le nom est null ou vide
            {
                return null;
            }

            nom = nom.Trim(); // Supprimer les espaces superflus

            return TousLesArtistes()
                .FirstOrDefault(a => string.Equals(a.Nom, nom, StringComparison.OrdinalIgnoreCase)); // Comparer les noms en ignorant la casse
        }
    }
}
