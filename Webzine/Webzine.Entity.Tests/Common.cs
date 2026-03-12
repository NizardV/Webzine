namespace Webzine.Entity.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Méthodes utilitaires permettant de tester les entités. 
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Vérifie que l'entité possède bien la propriété passée en paramètre.
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        public static void HasProperty(Type typeObjet, string nomPropriete)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            Assert.IsNotNull(property, "La classe " + typeObjet.Name + " doit avoir une propriété '" + nomPropriete + "'.");
        }

        /// <summary>
        /// Vérifie que l'attribut de l'entité a l'annotation [Display(Name = "xxx")] avec la valeur attendue.
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        /// <param name="chaineAttendue">valeur attendue pour l'affichage de cette propriété</param>
        public static void AttributDisplay(Type typeObjet, string nomPropriete, string chaineAttendue)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            var annotation = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();
            Assert.IsNotNull(annotation, "La propriété '" + nomPropriete + "' n'a pas de libellé approprié. Il manque l'annotation Display.");
            Assert.AreEqual(chaineAttendue, annotation.Name);
        }

        /// <summary>
        /// Vérifie que l'attribut de l'entité a l'annotation [MinLength(xx)] avec la longueur attendue.
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        /// <param name="max">longueur maximum</param>
        public static void AttributLongueurMax(Type typeObjet, string nomPropriete, int max)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            var annotation = (MaxLengthAttribute)property.GetCustomAttributes(typeof(MaxLengthAttribute), false).FirstOrDefault();
            Assert.IsNotNull(annotation, "La propriété '" + nomPropriete + "' n'a pas de longueur maximum. Il manque l'annotation MaxLength.");
            Assert.AreEqual(max, annotation.Length, "La propriété '" + nomPropriete + "' ne doit pas pouvoir dépasser " + max + " caractères.");
        }

        /// <summary>
        /// Vérifie que l'attribut de l'entité a l'annotation [MinLength(xx)] avec la longueur attendue.
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        /// <param name="min">longueur minimum</param>
        public static void AttributLongueurMin(Type typeObjet, string nomPropriete, int min)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            var annotation = (MinLengthAttribute)property.GetCustomAttributes(typeof(MinLengthAttribute), false).FirstOrDefault();
            Assert.IsNotNull(annotation, "La propriété '" + nomPropriete + "' n'a pas de longueur minimum. Il manque l'annotation MinLength.");
            Assert.AreEqual(min, annotation.Length, "La propriété '" + nomPropriete + "' ne avoir au moins " + min + " caractères.");
        }

        /// <summary>
        /// Vérifie que l'attribut de l'entité a l'annotation [Required].
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        public static void AttributRequis(Type typeObjet, string nomPropriete)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            var annotation = (RequiredAttribute)property.GetCustomAttributes(typeof(RequiredAttribute), false).FirstOrDefault();
            Assert.IsNotNull(annotation, "La propriété '" + nomPropriete + "' n'est pas obligatoire. Il manque l'annotation Required.");
        }

        /// <summary>
        /// Vérifie que l'attribut de l'entité n'a pas l'annotation [Url].
        /// </summary>
        /// <param name="typeObjet">type de l'entité</param>
        /// <param name="nomPropriete">nom de la propriété de l'entité</param>
        public static void AttributHasNotUrlValidation(Type typeObjet, string nomPropriete)
        {
            var property = typeObjet.GetProperty(nomPropriete);
            var annotation = (UrlAttribute)property.GetCustomAttributes(typeof(UrlAttribute), false).FirstOrDefault();
            Assert.IsNull(annotation, "La propriété '" + nomPropriete + "' ne doit pas être une URL obligatoirement. Retirez l'annotation Url.");
        }
    }
}
