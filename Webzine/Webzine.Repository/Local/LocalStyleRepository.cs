using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    public class LocalStyleRepository : IStyleRepository
    {
        /// <summary>
        /// Permet d'accéder à la liste des styles en mémoire. 
        /// Cette liste est utilisée pour stocker les styles de manière temporaire pendant l'exécution de l'application.
        /// </summary>
        private readonly WebzineData _data;

        private readonly ILogger<LocalStyleRepository> _logger;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="LocalStyleRepository"/> class.
        /// Constructeur de la classe LocalStyleRepository qui prend en paramètre une instance de WebzineData.
        /// </summary>
        /// <param name="data"></param>
        public LocalStyleRepository(WebzineData data, ILogger<LocalStyleRepository> logger)
        {
            _data = data;
            _logger = logger;
        }

        /// <summary>
        /// Ajoute un nouveau style de musique à la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        public void Add(Style style)
        {
            try
            {
                this._logger.LogDebug(
                    "Tentative d'ajout du style {Libelle} dans le repository local.",
                    style.Libelle);

                _data.styles.Add(style);

                this._logger.LogDebug(
                    "Style {Libelle} ajouté avec succès au repository local.",
                    style.Libelle);
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de l'ajout du style {Libelle} au repository local.",
                    style.Libelle);
                throw;
            }
        }

        /// <summary>
        /// Supprime un style de musique de la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        public void Delete(Style style)
        {
            try
            {
                this._logger.LogDebug(
                    "Tentative de suppression du style {IdStyle} du repository local.",
                    style.IdStyle);

                var styleASupprimer = _data.styles.FirstOrDefault(s => s.IdStyle == style.IdStyle);
                if (styleASupprimer != null)
                {
                    _data.styles.Remove(styleASupprimer);
                    this._logger.LogDebug(
                        "Style {IdStyle} supprimé avec succès du repository local.",
                        style.IdStyle);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la suppression du style {IdStyle} du repository local.",
                    style.IdStyle);
                throw;
            }
        }

        /// <summary>
        /// Retourne un style de musique dans la collection de styles en utilisant son identifiant unique.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Un objet style</returns>
        public Style Find(int id)
        {
            try
            {
                this._logger.LogDebug(
                    $"Recherche du style d'identifiant {id} dans le repository local.");

                var style = _data.styles.FirstOrDefault(s => s.IdStyle == id);

                if (style == null)
                {
                    this._logger.LogWarning(
                        "Aucun style trouvé dans le repository local pour l'identifiant {IdStyle}.",
                        id);
                }

                return style;
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la recherche du style {IdStyle} dans le repository local.",
                    id);
                throw;
            }
        }

        /// <summary>
        /// Retourne une liste de tous les styles de musique présents dans la collection de styles.
        /// </summary>
        /// <returns>Une liste de styles</returns>
        public IEnumerable<Style> FindAll()
        {
            try
            {
                this._logger.LogDebug(
                    "Récupération de tous les styles depuis le repository local.");

                return _data.styles.ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la récupération de tous les styles depuis le repository local.");
                throw;
            }
        }

        /// <summary>
        /// Met à jour les informations d'un style de musique existant dans la collection de styles.
        /// </summary>
        /// <param name="style"></param>
        public void Update(Style style)
        {
            try
            {
                this._logger.LogDebug(
                    "Tentative de modification du style {IdStyle} dans le repository local.",
                    style.IdStyle);

                var styleExistant = _data.styles.FirstOrDefault(s => s.IdStyle == style.IdStyle);
                if (styleExistant != null)
                {
                    styleExistant.Libelle = style.Libelle;
                    this._logger.LogDebug(
                        "Style {IdStyle} modifié avec succès dans le repository local.",
                        style.IdStyle);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la modification du style {IdStyle} dans le repository local.",
                    style.IdStyle);
                throw;
            }
        }
    }
}
