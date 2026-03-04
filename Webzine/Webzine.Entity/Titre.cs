namespace Webzine.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text;

    /// <summary>
    /// Model Titre représentant un titre de musique dans le contexte de l'application Webzine.
    /// </summary>
    [Table("Titre")]
    public class Titre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Libelle { get; set; }

        [Required]
        [MaxLength(255)]
        public string Chronique { get; set; }

        [Required]
        [MaxLength(255)]
        public string Lien { get; set; }

        [Required]
        public DateTime DateCreation { get; set; }

        [Required]
        public DateTime DateSortie { get; set; }

        [Required]
        public TimeOnly Duree { get; set; }
        
        public ICollection<Commentaire> Commentaires { get; set; }
        public ICollection<Style> Styles { get; set; }

    }
}
