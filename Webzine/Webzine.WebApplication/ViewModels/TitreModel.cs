namespace Webzine.WebApplication.ViewModels
{
    using Webzine.Entity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class TitreModel
    {
        /// <summary>
        /// Obtient ou définit une instance de la classe Titre qui représente les détails d'un titre spécifique.
        /// </summary>
        public Titre Titre { get; set; }

        [Display(Name = "Nom")]
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string Auteur { get; set; } = string.Empty;

        [Display(Name = "Commentaire")]
        [Required]
        [MinLength(10)]
        [MaxLength(1000)]
        public string Contenu { get; set; } = string.Empty;
    }
}
