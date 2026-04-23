using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webzine.Entity
{
    /// <summary>
    /// Entité représentant un commentaire publié sur un titre du webzine.
    /// Chaque commentaire est rattaché à un seul titre et contient
    /// un auteur, une date de création et un contenu.
    /// </summary>
    [Table("Commentaire")]
    public class Commentaire
    {
        /// <summary>
        /// Identifiant unique du commentaire en base de données,
        /// utilisé comme clé primaire.
        /// </summary>
        [Key]
        public int IdCommentaire { get; set; }

        /// <summary>
        /// Nom ou pseudo de l'auteur du commentaire.
        /// Cette valeur est obligatoire et doit contenir entre 2 et 30 caractères.
        /// </summary>
        [Required]
        [Display(Name = "Nom")]
        [MaxLength(30)]
        [MinLength(2)]
        public string Auteur { get; set; } = string.Empty;

        /// <summary>
        /// Date de création du commentaire.
        /// Cette valeur est obligatoire et correspond à la date de publication du commentaire.
        /// </summary>
        [Required]
        [Display(Name = "Date de création")]
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Contenu du commentaire.
        /// Cette valeur est obligatoire et doit contenir entre 10 et 1000 caractères.
        /// </summary>
        [MaxLength(1000)]
        [MinLength(10)]
        [Required]
        [Display(Name = "Commentaire")]
        public string Contenu { get; set; } = string.Empty;

        /// <summary>
        /// Identifiant du titre auquel le commentaire est rattaché.
        /// Cette propriété représente la clé étrangère vers l'entité <see cref="Titre"/>.
        /// </summary>
        [ForeignKey("Titre")]
        public int IdTitre { get; set; }

        /// <summary>
        /// Titre auquel le commentaire est associé.
        /// </summary>
        [Required]
        public Titre Titre { get; set; }
    }
}