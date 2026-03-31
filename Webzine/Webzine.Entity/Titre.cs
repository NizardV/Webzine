namespace Webzine.Entity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    /// <summary>
    /// Entité Titre réprésentant un titre musical.
    /// Chaque titre a un identifiant unique, un libellé,
    /// une chronique, une image de jaquette, un lien vers le son, des dates de création et de sortie,
    /// une durée, des compteurs de lecture et de like,
    /// une référence à un artiste et des collections de commentaires et de styles associés.
    /// </summary>
    [Table("Titre")]
    public class Titre
    {
        /// <summary>
        /// Identifiant unique du titre en base de données, utilisé comme clé primaire.
        /// </summary>
        [Key]
        public int IdTitre { get; set; }

        /// <summary>
        /// Libellé du titre, qui doit être fourni et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(200)]
        [MinLength(1)]
        [Display(Name = "Titre")]
        public string Libelle { get; set; }

        /// <summary>
        /// Chronique du titre, qui doit être fournie et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(4000)]
        [MinLength(10)]
        public string Chronique { get; set; }

        /// <summary>
        /// Image de jaquette du titre, qui doit être fournie et représente le visuel associé au titre.
        /// </summary>
        [Required]
        [Display(Name = "Jaquette de l'album")]
        [MaxLength(250)]
        public string UrlJaquette { get; set; }

        /// <summary>
        /// Lien vers le son du titre, qui doit être fourni et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(250)]
        [MinLength(13)]
        [Display(Name = "URL d'écoute")]
        public string UrlEcoute { get; set; }

        /// <summary>
        /// Date de création du titre,
        /// qui doit être fournie et représente la date à laquelle le titre a été créé ou ajouté à la base de données.
        /// </summary>
        [Required]
        [Display(Name = "Date de création")]
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Date de sortie du titre,
        /// qui doit être fournie et représente la date à laquelle le titre a été publié ou rendu disponible au public.
        /// </summary>
        [Required]
        [Display(Name = "Date de sortie")]
        public DateTime DateSortie { get; set; }

        /// <summary>
        /// Durée du titre, qui doit être fournie et représente la durée totale du titre musical.
        /// </summary>
        [Required]
        [Display(Name = "Durée en secondes")]
        public TimeOnly Duree { get; set; }

        /// <summary>
        /// Nombre de lectures du titre.
        /// Cette valeur est incrémentée à chaque consultation de la page du titre.
        /// </summary>
        [Required]
        [Display(Name = "Nombre de lectures")]
        public int NbLectures { get; set; }

        /// <summary>
        /// Nombre de likes du titre.
        /// Cette valeur représente le nombre de fois où le titre a été apprécié par les visiteurs.
        /// </summary>
        [Required]
        [Display(Name = "Nombre de likes")]
        public int NbLikes { get; set; }

        /// <summary>
        /// Identifiant de l'artiste associé au titre.
        /// Cette propriété représente la clé étrangère vers l'entité <see cref="Artiste"/>.
        /// </summary>
        [ForeignKey("Artiste")]
        public int IdArtiste { get; set; }

        /// <summary>
        /// Artiste associé au titre.
        /// </summary>
        public Artiste Artiste { get; set; }

        /// <summary>
        /// Nom de l'album auquel appartient le titre.
        /// </summary>
        [Required]
        public string Album { get; set; } = string.Empty;

        /// <summary>
        /// Obtient ou définit la collection de commentaires associés à l'entité.
        /// </summary>
        /// <remarks>Modifier la collection directement affecte les commentaires associés à l'objet.</remarks>
        public ICollection<Commentaire> Commentaires { get; set; }

        /// <summary>
        /// Obtient ou définit la collection de styles associés à l'entité.
        /// </summary>
        /// <remarks>Modifier la collection directement affecte les styles associés à l'objet.</remarks>
        public ICollection<Style> Styles { get; set; }

    }
}
