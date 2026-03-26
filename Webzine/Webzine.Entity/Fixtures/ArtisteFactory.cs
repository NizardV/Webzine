namespace Webzine.Entity.Fixtures
{
    using Bogus;

    /// <summary>
    /// Factory permettant de créer des faux artistes.
    /// </summary>
    public class ArtisteFactory
    {

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

            var artistes = new List<Artiste>();

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

    }
}
