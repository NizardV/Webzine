using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;

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

                IdTitre = 1,
                Libelle = "Titre 1",
                Chronique = "Chronique du titre 1",
                UrlJaquette = "image1.jpg",
                UrlEcoute = "https://example.com/titre1",
                DateCreation = DateTime.Now,
                DateSortie = DateTime.Now.AddMonths(1),
                Duree = new TimeOnly(3, 45, 00),
                NbLectures = 0,
                NbLikes = 0,
                IdArtiste = 2,
                Album = "Album 1",
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                IdTitre = 2,
                Libelle = "Titre 2",
                Chronique = "Chronique du titre 2",
                UrlJaquette = "image1.jpg",
                UrlEcoute = "https://example.com/titre2",
                DateCreation = DateTime.Now,
                DateSortie = DateTime.Now.AddMonths(2),
                Duree = new TimeOnly(4, 20, 00),
                NbLectures = 0,
                NbLikes = 0,
                IdArtiste = 2,
                Album = "Album 1",
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                IdTitre = 3,
                Libelle = "Titre 3",
                Chronique = "Chronique du titre 3",
                UrlJaquette = "image2.jpg",
                UrlEcoute = "https://example.com/titre3",
                DateCreation = DateTime.Now,
                DateSortie = DateTime.Now.AddMonths(3),
                Duree = new TimeOnly(5, 10, 00),
                NbLectures = 0,
                NbLikes = 0,
                IdArtiste = 2,
                Album = "Album 2",
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                IdTitre = 4,
                Libelle = "Titre 4",
                Chronique = "Chronique du titre 4",
                UrlJaquette = "image3.jpg",
                UrlEcoute = "https://example.com/titre4",
                DateCreation = DateTime.Now,
                DateSortie = DateTime.Now.AddMonths(4),
                Duree = new TimeOnly(2, 3, 00),
                NbLectures = 0,
                NbLikes = 0,
                IdArtiste = 1,
                Album = "Album 3",
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            return titres;
        }

        /// <summary>
        /// Récupérer une liste de titres associés à un artiste spécifique en fonction de l'identifiant de l'artiste.
        /// </summary>
        /// <param name="idArtiste">L'identifiant de l'artiste</param>
        /// <returns>Liste de titres fait par l'artiste en paramètre</returns>
        public static List<Titre> TrouverParIdArtiste(int idArtiste)
        {
            return CreerTitres().Where(t => t.IdArtiste == idArtiste).ToList();
        }
    }
}
