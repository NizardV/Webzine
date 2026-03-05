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
        public int IdTitre { get; set; }

        /// <summary>
        /// Gets or sets le libellé ou nom du titre, qui doit être fourni et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        [Display(Name = "Titre")]
        public string Libelle { get; set; }

        /// <summary>
        /// Gets or sets la chronique ou description du titre, qui doit être fournie et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(4000)]
        [MinLength(10)]
        public string Chronique { get; set; }

        [Required]
        [Display(Name = "Jaquette de l'album")]
        [MaxLength(250)]
        public string UrlJaquette { get; set; }

        [Required]
        [MaxLength(250)]
        [MinLength(13)]
        [Display(Name = "URL d'écoute")]
        public string UrlEcoute { get; set; }

        [Required]
        [Display(Name = "Date de création")]
        public DateTime DateCreation { get; set; }

        [Required]
        [Display(Name = "Date de sortie")]
        public DateTime DateSortie { get; set; }

        [Required]
        [Display(Name = "Durée en secondes")]
        public TimeOnly Duree { get; set; }

        [Required]
        [Display(Name = "Nombre de lectures")]
        public int NbLectures { get; set; }

        [Required]
        [Display(Name = "Nombre de likes")]
        public int NbLikes { get; set; }

        [ForeignKey("Artiste")]
        public int IdArtiste { get; set; }
        public Artiste Artiste { get; set; }

        [Required]
        public string Album { get; set; } = string.Empty;

        public ICollection<Commentaire> Commentaires { get; set; }

        public ICollection<Style> Styles { get; set; }

    }
}
