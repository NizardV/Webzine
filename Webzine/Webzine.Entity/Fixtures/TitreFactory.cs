//using System;
//using System.Collections.Generic;
//using System.Text;
//using Webzine.Entity;

namespace Webzine.Entity.Fixtures
{
    /// <summary>
    /// Factory pour la création d'instances de la classe Titre.
    /// Cette classe peut être utilisée pour centraliser la logique de création d'objets Titre,
    /// notamment si la création de ces objets nécessite des étapes supplémentaires ou des valeurs par défaut.
    /// </summary>
    public class TitreFactory
    {
        private readonly ICollection<Titre> titres = new List<Titre>()
        {
            new Titre
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
            },
            new Titre
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
            },
        };

        /// <summary>
        /// Retourne une liste de titres préconfigurés. Cette méthode peut être utilisée pour obtenir des instances de Titre
        /// </summary>
        /// <returns>Liste de titres</returns>
        public static ICollection<Titre> GetTitres()
        {
            return new TitreFactory().titres;
        }

        /// <summary>
        /// Retourne un titre spécifique en fonction de son identifiant. Cette méthode peut être utilisée pour obtenir une instance de Titre.
        /// </summary>
        /// <param name="id">Identifiant du titre</param>
        /// <returns>Titre</returns>
        public static Titre? GetTitreById(int id)
        {
            return new TitreFactory().titres.FirstOrDefault(t => t.IdTitre == id);
        }
    }
}
