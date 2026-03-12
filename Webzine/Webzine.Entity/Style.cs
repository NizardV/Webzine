using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;

namespace Webzine.Entity
{

    /// <summary>
    /// Représente un style musical (ex: Rock, Jazz, Électro).
    /// </summary>
    [Table("Style")]
    public class Style
    {
        /// <summary>
        /// Identifiant unique du style en base de données.
        /// </summary>
        [Key]
        public int IdStyle { get; set; }

        /// <summary>
        /// Nom ou libellé affichable du style.
        /// </summary>
        [Required]
        [Display(Name = "Libellé")]
        [MaxLength(50)]
        [MinLength(2)]
        public string Libelle { get; set; } = string.Empty;

        /// <summary>
        /// Liste des titres associés au style.
        /// </summary>
        public virtual ICollection<Titre> Titres { get; set; } = new List<Titre>();
    }

}
