namespace Webzine.Entity.Fixtures
{
    using Bogus;

    /// <summary>
    /// Factory permettant de créer des faux artistes.
    /// </summary>
    public class ArtisteFactory
    {
        private const int DefaultCount = 8;

        private static readonly IReadOnlyList<Artiste> BaseArtistes =
        [
            new Artiste { IdArtiste = 1, Nom = "Serge Gainsbourg", Biographie = "Pionnier de la variété française." },
            new Artiste { IdArtiste = 2, Nom = "Daft Punk", Biographie = "Duo casqué pionnier de la French Touch, mêlant house, synthpop et disco." },
            new Artiste { IdArtiste = 3, Nom = "Boards of Canada", Biographie = "Frères écossais célèbres pour leurs mélodies nostalgiques et leurs samples obscurs." },
            new Artiste { IdArtiste = 4, Nom = "Bicep", Biographie = "Duo nord-irlandais incontournable de la scène house et techno contemporaine." },
            new Artiste { IdArtiste = 5, Nom = "Kraftwerk", Biographie = "Pionniers allemands absolus de la musique électronique dès les années 1970." },
            new Artiste { IdArtiste = 6, Nom = "Burial", Biographie = "Producteur londonien insaisissable, figure de proue du genre dubstep et future garage." },
            new Artiste { IdArtiste = 7, Nom = "Peggy Gou", Biographie = "DJ et productrice sud-coréenne, créatrice d'une house énergique aux influences acid." },
            new Artiste { IdArtiste = 8, Nom = "Laurent Garnier", Biographie = "DJ historique et pilier de la scène techno et house en France." },
        ];

        /// <summary>
        /// Récupérer tous les artistes.
        /// </summary>
        /// <return>Liste contenant des artistes</return>
        public static List<Artiste> CreerArtistes(int numberOfData)
        {
            if (numberOfData <= 0)
            {
                return [];
            }

            var artistes = BaseArtistes
                .Take(numberOfData)
                .Select(a => new Artiste
                {
                    IdArtiste = a.IdArtiste,
                    Nom = a.Nom,
                    Biographie = a.Biographie,
                })
                .ToList();

            if (artistes.Count >= numberOfData)
            {
                return artistes;
            }

            var faker = new Faker<Artiste>("fr").UseSeed(1337)
                .RuleFor(a => a.Nom, f => Truncate(f.Name.FullName(), 50))
                .RuleFor(a => a.Biographie, f => f.Lorem.Sentences(2));

            var complement = faker.Generate(numberOfData - artistes.Count);
            var id = artistes.Count + 1;
            foreach (var artiste in complement)
            {
                artiste.IdArtiste = id++;
                if (string.IsNullOrWhiteSpace(artiste.Nom) || artiste.Nom.Length < 2)
                {
                    artiste.Nom = "Artiste";
                }

                artistes.Add(artiste);
            }

            return artistes;
        }

        private static string Truncate(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            return value.Length <= maxLength ? value : value[..maxLength];
        }

        /// <summary>
        /// Récupérer un artiste par son id
        /// </summary>
        /// <param name="artisteId">L'identifiant de l'artiste</param>
        /// <return>Un object artiste</return>
        public static Artiste? TrouverParId(int artisteId)
        {
            if (artisteId <= 0)
            {
                return null;
            }

            var count = Math.Max(DefaultCount, artisteId);
            return CreerArtistes(count).FirstOrDefault(a => a.IdArtiste == artisteId);
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

            nom = nom.Trim();

            return CreerArtistes(DefaultCount)
                .FirstOrDefault(a => string.Equals(a.Nom, nom, StringComparison.OrdinalIgnoreCase)); // Comparer les noms en ignorant la casse
        }
    }
}
