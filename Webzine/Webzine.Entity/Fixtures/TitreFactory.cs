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
        public static ICollection<Titre> CreerTitres(List<Style> styles, List<Artiste> artistes, int nombreDeTitres)
        {
            var titres = new List<Titre>();
            var faker = new Faker<Titre>()
                .RuleFor(t => t.IdTitre, (f, t) => f.IndexFaker + 1)
                .RuleFor(t => t.Libelle, f => f.Lorem.Word())
                .RuleFor(t => t.Chronique, f => f.Lorem.Paragraph())
                .RuleFor(t => t.UrlJaquette, f => f.Internet.Avatar())
                .RuleFor(t => t.UrlEcoute, f => f.Internet.Url())
                .RuleFor(t => t.DateCreation, f => f.Date.Past())
                .RuleFor(t => t.DateSortie, (f, t) => f.Date.Between(t.DateCreation, DateTime.Now))
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
                });
            titres = faker.Generate(nombreDeTitres > 0 ? nombreDeTitres : 50);
            return titres;
        }
    }
}
