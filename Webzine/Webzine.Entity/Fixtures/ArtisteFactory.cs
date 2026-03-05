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
                Biographie = "The Beatles sont un groupe britannique formé à Liverpool en 1960, composé de John Lennon, Paul McCartney, George Harrison et Ringo Starr. Ils ont révolutionné la musique populaire avec des mélodies innovantes, des expérimentations en studio et une influence mondiale énorme. Des titres comme Hey Jude, Let It Be ou Yesterday ont traversé les générations, et leur impact sur la pop/rock reste colossal.",
            });

            artistes.Add(new Artiste
            {
                IdArtiste = 2,
                Nom = "Eminem",
                Biographie = "Eminem (Marshall Bruce Mathers III), né en 1972 à Detroit, est l’un des rappeurs les plus influents de l’histoire. Révélé à la fin des années 1990 avec The Slim Shady LP, il se distingue par son écriture technique, ses alter egos, et des textes souvent introspectifs. Il a popularisé le rap auprès d’un très large public et a marqué la culture pop avec des albums comme The Marshall Mathers LP et The Eminem Show.",
            });

            artistes.Add(new Artiste
            {
                IdArtiste = 3,
                Nom = "Stromae",
                Biographie = "Stromae (Paul Van Haver), né en 1985 en Belgique, est un auteur-compositeur-interprète connu pour son mélange unique d’électro, de pop et d’influences urbaines. Il s’est fait connaître avec Alors on danse, puis a confirmé son succès avec des albums comme Racine carrée et Multitude. Son style se démarque par des textes forts (solitude, famille, société) et une identité visuelle très travaillée.",
            });

            return artistes;
        }

        /// <summary>
        /// Récupérer un artiste par son id
        /// </summary>
        /// <param name="artisteId">L'identifiant de l'artiste</param>
        /// <return>Un object artiste</return>
        public static Artiste? TrouverParId(int artisteId)
        {
            return TousLesArtistes().FirstOrDefault(a => a.IdArtiste == artisteId);
        }

        /// <summary>
        /// Récupérer un artiste par son nom.
        /// </summary>
        /// <param name="nom">Le nom de l'artiste</param>
        /// <return>Un object artiste</return>
        public static Artiste? TrouverParNom(string nom)
        {
            if (string.IsNullOrWhiteSpace(nom)) // Vérifier si le nom est null ou vide
            {
                return null;
            }

            nom = nom.Trim(); // Supprimer les espaces superflus

            return TousLesArtistes().FirstOrDefault(a => 
            string.Equals(a.Nom, nom, StringComparison.OrdinalIgnoreCase)); // Comparer les noms en ignorant la casse
        }
    }
}
