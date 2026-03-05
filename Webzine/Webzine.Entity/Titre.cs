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
        public int Id { get; set; }

        /// <summary>
        /// Libellé du titre, qui doit être fourni et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Libelle { get; set; }

        /// <summary>
        /// Chronique du titre, qui doit être fournie et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Chronique { get; set; }

        /// <summary>
        /// Image de jaquette du titre, qui doit être fournie et représente le visuel associé au titre.
        /// </summary>
        [Required]
        public string ImageJaquette { get; set; }

        /// <summary>
        /// Lien vers le son du titre, qui doit être fourni et ne peut pas dépasser 255 caractères.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public string Lien { get; set; }

        /// <summary>
        /// Date de création du titre,
        /// qui doit être fournie et représente la date à laquelle le titre a été créé ou ajouté à la base de données.
        /// </summary>
        [Required]
        public DateTime DateCreation { get; set; }

        /// <summary>
        /// Date de sortie du titre,
        /// qui doit être fournie et représente la date à laquelle le titre a été publié ou rendu disponible au public.
        /// </summary>
        [Required]
        public DateTime DateSortie { get; set; }

        /// <summary>
        /// Durée du titre, qui doit être fournie et représente la durée totale du titre musical.
        /// </summary>
        [Required]
        public TimeOnly Duree { get; set; }

        /// <summary>
        /// Compteur de lecture du titre qui représente le nombre de fois que le titre a été écouté ou lu.
        /// </summary>
        public int CompteurLecture { get; set; }

        /// <summary>
        /// Compteur de like du titre qui représente le nombre de fois que le titre a été aimé ou apprécié par les utilisateurs.
        /// </summary>
        public int CompteurLike { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'artiste associé à l'album.
        /// </summary>
        /// <remarks>Cette propriété est utilisée comme clé étrangère pour établir la relation entre un
        /// album et son artiste dans la base de données.</remarks>
        [ForeignKey("Artiste")]
        public int ArtisteId { get; set; }

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
