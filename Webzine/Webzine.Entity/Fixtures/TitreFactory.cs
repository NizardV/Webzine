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
        /// Créer une liste d'instances de la classe Titre avec des données d'exemple.
        /// </summary>
        /// <returns>
        /// Liste d'instances de la classe Titre créée.
        /// </returns>
        public static List<Titre> GetTitres()
        {
            var titres = new List<Titre>();

            titres.Add(new Titre
            {
                IdTitre = 1,
                Libelle = "Titre 1",
                Chronique = "Chronique du titre 1",
                UrlJaquette = "image1.jpg",
                UrlEcoute = "https://example.com/titre1",
                DateCreation = DateTime.Now,
                DateSortie = DateTime.Now.AddMonths(1),
                NbLectures = 0,
                NbLikes = 0,
                IdArtiste = 1,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                IdTitre = 2,
                Libelle = "Titre 2",
                Chronique = "Chronique du titre 2",
                UrlJaquette = "image2.jpg",
                UrlEcoute = "https://example.com/titre2",
                DateCreation = DateTime.Now,
                DateSortie = DateTime.Now.AddMonths(2),
                NbLectures = 0,
                NbLikes = 0,
                IdArtiste = 1,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                IdTitre = 6,
                Libelle = "Techno Boulevard",
                Chronique = "Kick sec, hats incisifs, montée progressive: techno fonctionnelle.",
                UrlJaquette = "jaquette-techno-boulevard.jpg",
                UrlEcoute = "https://example.com/titres/techno-boulevard",
                DateCreation = DateTime.Now.AddDays(-4),
                DateSortie = DateTime.Now.AddDays(-1),
                NbLectures = 733,
                NbLikes = 89,
                IdArtiste = 5,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                IdTitre = 7,
                Libelle = "Nu-Disco Sunrise",
                Chronique = "Claps, cordes et groove solaire: un nu-disco sans complexe.",
                UrlJaquette = "jaquette-nu-disco-sunrise.jpg",
                UrlEcoute = "https://example.com/titres/nu-disco-sunrise",
                DateCreation = DateTime.Now.AddMonths(-3).AddDays(2),
                DateSortie = DateTime.Now.AddMonths(-3).AddDays(5),
                NbLectures = 312,
                NbLikes = 41,
                IdArtiste = 1,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                IdTitre = 8,
                Libelle = "Drum'n'Bass Postcard",
                Chronique = "Breaks rapides et sub massif, mais avec une mélodie catchy.",
                UrlJaquette = "jaquette-dnb-postcard.jpg",
                UrlEcoute = "https://example.com/titres/dnb-postcard",
                DateCreation = DateTime.Now.AddDays(-40),
                DateSortie = DateTime.Now.AddDays(-35),
                NbLectures = 1280,
                NbLikes = 156,
                IdArtiste = 6,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            return titres;
        }

        public static Titre? GetTitreById(int id)
        {
            return GetTitres().FirstOrDefault(t => t.IdTitre == id);
        }
    }
}