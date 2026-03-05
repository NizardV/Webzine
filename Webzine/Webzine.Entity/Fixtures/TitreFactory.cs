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
                Id = 8,
                Libelle = "Drum'n'Bass Postcard",
                Chronique = "Breaks rapides et sub massif, mais avec une mélodie catchy.",
                ImageJaquette = "jaquette-dnb-postcard.jpg",
                Lien = "https://example.com/titres/dnb-postcard",
                DateCreation = DateTime.Now.AddDays(-40),
                DateSortie = DateTime.Now.AddDays(-35),
                CompteurLecture = 1280,
                CompteurLike = 156,
                ArtisteId = 6,
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
