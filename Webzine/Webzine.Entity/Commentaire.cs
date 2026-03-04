using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webzine.Entity
{
    [Table("Commentaire")]
    public class Commentaire
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Pseudo { get; set; } = string.Empty;

        [Required]
        public DateOnly DateCreation { get; set; } = DateOnly.FromDateTime(DateTime.Now);

        [MaxLength(200)]
        public string Libelle { get; set; } = string.Empty;

        [ForeignKey(nameof(Titre.Id))]
        public int TitreId { get; set; }
    }
}
