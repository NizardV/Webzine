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
        public static List<Artiste> CreerArtistes()
        {
            var artistes = new List<Artiste>();

            artistes.Add(new Artiste { IdArtiste = 1, Nom = "Aphex Twin", Biographie = "Légende de la musique électronique, maître de l'IDM et de l'expérimentation sonore." });
            artistes.Add(new Artiste { IdArtiste = 2, Nom = "Daft Punk", Biographie = "Duo casqué pionnier de la French Touch, mêlant house, synthpop et disco." });
            artistes.Add(new Artiste { IdArtiste = 3, Nom = "Boards of Canada", Biographie = "Frères écossais célèbres pour leurs mélodies nostalgiques et leurs samples obscurs." });
            artistes.Add(new Artiste { IdArtiste = 4, Nom = "Bicep", Biographie = "Duo nord-irlandais incontournable de la scène house et techno contemporaine." });
            artistes.Add(new Artiste { IdArtiste = 5, Nom = "Kraftwerk", Biographie = "Pionniers allemands absolus de la musique électronique dès les années 1970." });
            artistes.Add(new Artiste { IdArtiste = 6, Nom = "Burial", Biographie = "Producteur londonien insaisissable, figure de proue du genre dubstep et future garage." });
            artistes.Add(new Artiste { IdArtiste = 7, Nom = "Peggy Gou", Biographie = "DJ et productrice sud-coréenne, créatrice d'une house énergique aux influences acid." });
            artistes.Add(new Artiste { IdArtiste = 8, Nom = "Laurent Garnier", Biographie = "DJ historique et pilier de la scène techno et house en France." });

            return artistes;
        }

        /// <summary>
        /// Récupérer un artiste par son id
        /// </summary>
        /// <return>Un object artiste</return>
        public static Artiste? TrouverParId(int artisteId)
        {
            return CreerArtistes().FirstOrDefault(a => a.IdArtiste == artisteId);
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

            nom = nom.Trim();

            return CreerArtistes()
                .FirstOrDefault(a => string.Equals(a.Nom, nom, StringComparison.OrdinalIgnoreCase)); // Comparer les noms en ignorant la casse
        }
    }
}
