using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webzine.Entity
{
    /// <summary>
    /// Entité Artiste modélisant un artiste.
    /// Chaque artiste a un identifiant unique, un nom, une biographie et une collection de titres associés.
    /// </summary>
    [Table("Artiste")]
    public class Artiste
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de l'artiste.
        /// </summary>
        [Key]
        public int IdArtiste { get; set; }

        /// <summary>
        /// Obtient ou définit le nom de l'artiste.
        /// </summary>
        [Required]
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou définit la biographie de l'artiste.
        /// </summary>
        public string Biographie { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des titres associés à l'artiste.
        /// </summary>
        public ICollection<Titre> Titres { get; set; } = new List<Titre>();
    }
}
