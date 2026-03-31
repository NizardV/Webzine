using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webzine.Entity
{
    [Table("Commentaire")]
    public class Commentaire
    {
        [Key]
        public int IdCommentaire { get; set; }

        [Required]
        [Display(Name = "Nom")]
        [MaxLength(30)]
        [MinLength(2)]
        public string Auteur { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Date de création")]
        public DateOnly DateCreation { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [MaxLength(1000)]
        [MinLength(10)]
        [Required]
        [Display(Name = "Commentaire")]
        public string Contenu { get; set; } = string.Empty;

        [ForeignKey("Titre")]
        public int IdTitre { get; set; }

        [Required]
        public Titre Titre { get; set; }
    }
}
