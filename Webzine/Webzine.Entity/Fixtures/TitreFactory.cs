using Bogus;

namespace Webzine.Entity.Fixtures
{
    /// <summary>
    /// Factory pour la création d'instances de la classe Titre.
    /// Cette classe peut être utilisée pour centraliser la logique de création d'objets Titre,
    /// notamment si la création de ces objets nécessite des étapes supplémentaires ou des valeurs par défaut.
    /// </summary>
    public class TitreFactory
    {
        /// <summary>
        /// Retourne une liste de titres préconfigurés. Cette méthode peut être utilisée pour obtenir des instances de Titre
        /// </summary>
        /// <returns>Liste de titres</returns>
        public static List<Titre> CreerTitres(List<Style> styles, List<Artiste> artistes, int nombreDeTitres)
        {
            var titres = new List<Titre>();
            var faker = new Faker<Titre>()
                .RuleFor(t => t.IdTitre, (f, t) => f.IndexFaker + 1)
                .RuleFor(t => t.Libelle, f => f.Lorem.Word())
                .RuleFor(t => t.Chronique, f => f.Lorem.Paragraph())
                .RuleFor(t => t.UrlJaquette, f => f.Image.PicsumUrl())
                .RuleFor(t => t.UrlEcoute, f => $"https://www.youtube.com/embed/{f.Random.String2(11, "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_")}")
                .RuleFor(t => t.DateCreation, f => f.Date.Past())
                .RuleFor(t => t.DateSortie, (f, t) => f.Date.Between(t.DateCreation, DateTime.UtcNow))
                .RuleFor(t => t.Duree, (f, t) => TimeOnly.FromTimeSpan(TimeSpan.FromSeconds(f.Random.Int(120, 420))))
                .RuleFor(t => t.Artiste, f => f.PickRandom(artistes))
                .RuleFor(t => t.IdArtiste, (f, t) => t.Artiste.IdArtiste)
                .RuleFor(t => t.NbLectures, f => f.Random.Int(0, 10000))
                .RuleFor(t => t.NbLikes, f => f.Random.Int(0, 5000))
                .RuleFor(t => t.Album, f => f.Lorem.Word())
                .RuleFor(t => t.Commentaires, f => new List<Commentaire>())
                .RuleFor(t => t.Styles, f => new List<Style>())
                .FinishWith((f, t) =>
                {
                    int nbStyles = f.Random.Int(1, 2);
                    var selectedStyles = f.PickRandom(styles, nbStyles);
                    foreach (var style in selectedStyles)
                    {
                        t.Styles.Add(style);
                        style.Titres.Add(t);
                    }
                    artistes.First(a => a.IdArtiste == t.IdArtiste).Titres.Add(t);
                });
            titres = faker.Generate(nombreDeTitres > 0 ? nombreDeTitres : 50);
            return titres;
        }

        public static List<Titre> AssignerStylesATitres(List<Titre> titres, List<Style> styles)
        {
            var random = new Random();
            foreach (var titre in titres)
            {
                int nbStyles = random.Next(1, 3);
                var selectedStyles = styles.OrderBy(s => random.Next()).Take(nbStyles).ToList();
                titre.Styles = selectedStyles;
                foreach (var style in selectedStyles)
                {
                    style.Titres.Add(titre);
                }
            }
            return titres;
        }
    }
}
