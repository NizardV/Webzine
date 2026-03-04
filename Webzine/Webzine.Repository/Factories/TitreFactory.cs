using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;

namespace Webzine.Repository.Factories
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
                Libelle = "Titre 1",
                Chronique = "Chronique du titre 1",
                ImageJaquette = "image1.jpg",
                Lien = "https://example.com/titre1",
                DateCreation = DateTime.Now,
                DateSortie = DateTime.Now.AddMonths(1),
                CompteurLecture = 0,
                CompteurLike = 0,
                ArtisteId = 1,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            titres.Add(new Titre
            {
                Id = 2,
                Libelle = "Titre 2",
                Chronique = "Chronique du titre 2",
                ImageJaquette = "image2.jpg",
                Lien = "https://example.com/titre2",
                DateCreation = DateTime.Now,
                DateSortie = DateTime.Now.AddMonths(2),
                CompteurLecture = 0,
                CompteurLike = 0,
                ArtisteId = 1,
                Commentaires = new List<Commentaire>(),
                Styles = new List<Style>(),
            });

            return titres;
        }
    }
}
