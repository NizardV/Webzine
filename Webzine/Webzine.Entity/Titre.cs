namespace Webzine.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Model Titre représentant un titre de musique dans le contexte de l'application Webzine.
    /// </summary>
    [Table("Titre")]
    public class Titre
    {
        /// <summary>
        /// Gets or sets l'identifiant unique du titre en base de données.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets le libellé ou nom du titre, qui doit être fourni et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Libelle { get; set; }

        /// <summary>
        /// Gets or sets la chronique ou description du titre, qui doit être fournie et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Chronique { get; set; }

        [Required]
        public string ImageJaquette { get; set; }

        [Required]
        [MaxLength(255)]
        public string Lien { get; set; }

        [Required]
        public DateTime DateCreation { get; set; }

        [Required]
        public DateTime DateSortie { get; set; }

        [Required]
        public TimeOnly Duree { get; set; }

        public int CompteurLecture { get; set; }

        public int CompteurLike { get; set; }

        [ForeignKey("Album")]
        public int ArtisteId { get; set; }

        public ICollection<Commentaire> Commentaires { get; set; }

        public ICollection<Style> Styles { get; set; }

    }
}
