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
        public static List<Titre> CreerTitres()
        {
            var titres = new List<Titre>();

            titres.Add(new Titre
            {
                Id = 1,
                Libelle = "Midnight Drive",
                Chronique = "Un titre électro nocturne, synthés rétro et basse bien grasse.",
                ImageJaquette = "jaquette-midnight-drive.jpg",
                Lien = "https://example.com/titres/midnight-drive",
                DateCreation = DateTime.Now.AddDays(-12),
                DateSortie = DateTime.Now.AddDays(-10),
                CompteurLecture = 154,
                CompteurLike = 23,
                ArtisteId = 1,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                Id = 2,
                Libelle = "Deep Blue",
                Chronique = "House profonde et planante, idéale pour un set de fin de soirée.",
                ImageJaquette = "jaquette-deep-blue.jpg",
                Lien = "https://example.com/titres/deep-blue",
                DateCreation = DateTime.Now.AddDays(-25),
                DateSortie = DateTime.Now.AddDays(-20),
                CompteurLecture = 981,
                CompteurLike = 102,
                ArtisteId = 2,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                Id = 3,
                Libelle = "Acid Skies",
                Chronique = "Acid assumée, 303 en avant, groove crunchy et efficace.",
                ImageJaquette = "jaquette-acid-skies.jpg",
                Lien = "https://example.com/titres/acid-skies",
                DateCreation = DateTime.Now.AddDays(-8),
                DateSortie = DateTime.Now.AddDays(3),
                CompteurLecture = 42,
                CompteurLike = 7,
                ArtisteId = 3,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                Id = 4,
                Libelle = "Downtown Samba",
                Chronique = "Percus légères, vibe lounge et un refrain qui reste en tête.",
                ImageJaquette = "jaquette-downtown-samba.jpg",
                Lien = "https://example.com/titres/downtown-samba",
                DateCreation = DateTime.Now.AddMonths(-2).AddDays(-2),
                DateSortie = DateTime.Now.AddMonths(-2),
                CompteurLecture = 2105,
                CompteurLike = 318,
                ArtisteId = 4,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                Id = 5,
                Libelle = "Ambient Letters",
                Chronique = "Nap duveteux, textures granuleuses, parfait pour respirer.",
                ImageJaquette = "jaquette-ambient-letters.jpg",
                Lien = "https://example.com/titres/ambient-letters",
                DateCreation = DateTime.Now.AddMonths(-1).AddDays(-10),
                DateSortie = DateTime.Now.AddMonths(-1).AddDays(-7),
                CompteurLecture = 507,
                CompteurLike = 64,
                ArtisteId = 2,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                Id = 6,
                Libelle = "Techno Boulevard",
                Chronique = "Kick sec, hats incisifs, montée progressive: techno fonctionnelle.",
                ImageJaquette = "jaquette-techno-boulevard.jpg",
                Lien = "https://example.com/titres/techno-boulevard",
                DateCreation = DateTime.Now.AddDays(-4),
                DateSortie = DateTime.Now.AddDays(-1),
                CompteurLecture = 733,
                CompteurLike = 89,
                ArtisteId = 5,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                Id = 7,
                Libelle = "Nu-Disco Sunrise",
                Chronique = "Claps, cordes et groove solaire: un nu-disco sans complexe.",
                ImageJaquette = "jaquette-nu-disco-sunrise.jpg",
                Lien = "https://example.com/titres/nu-disco-sunrise",
                DateCreation = DateTime.Now.AddMonths(-3).AddDays(2),
                DateSortie = DateTime.Now.AddMonths(-3).AddDays(5),
                CompteurLecture = 312,
                CompteurLike = 41,
                ArtisteId = 1,
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
    }
}
