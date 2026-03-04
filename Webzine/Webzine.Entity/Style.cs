using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Timers;

namespace Webzine.Entity
{
    [Table("Style")]
    public class Style 
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Libelle { get; set; } = string.Empty;


        public virtual ICollection<Titre> Titres { get; set; } = new List<Titre>();
    }
}
