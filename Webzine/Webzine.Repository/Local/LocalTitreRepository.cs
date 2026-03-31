using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    /// <inheritdoc/>
    public class LocalTitreRepository : ITitreRepository
    {
        public readonly WebzineData _data;
        private readonly ILogger<LocalTitreRepository> _logger;

        /// <inheritdoc/>
        public LocalTitreRepository(WebzineData data, ILogger<LocalTitreRepository> logger)
        {
            this._data = data;
            this._logger = logger;
        }

        /// <inheritdoc/>
        public void Add(Titre titre)
        {
            try
            {
                _data.titres.Add(titre);
                this._logger.LogDebug($"Titre {titre.Libelle} ajouté au repository local.");
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Erreur lors de l'ajout du titre {titre.Libelle} au repository local.", ex);
                throw;
            }
        }
        public int Count()
        {
            return this._data.titres.Count();
        }

        /// <inheritdoc/>
        public void Delete(Titre titre)
        {
            try
            {
                var titreASupprimer = _data.titres.FirstOrDefault(t => t.IdTitre == titre.IdTitre);
                if (titreASupprimer != null)
                {
                    _data.titres.Remove(titreASupprimer);
                    this._logger.LogDebug($"Titre {titre.IdTitre} supprimé du repository local.");
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Erreur lors de la suppression du titre {titre.IdTitre} du repository local.", ex);
                throw;
            }
        }

        /// <inheritdoc/>
        public Titre Find(int idTitre)
        {
            try
            {
                var titre = this._data.titres.FirstOrDefault(t => t.IdTitre == idTitre);
                this._logger.LogDebug("Recherche du titre {idTitre} - {Libelle}", idTitre, titre.Libelle);
                
                return titre;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche du titre {idTitre}", idTitre);
                throw;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Titre> FindTitres(int offset, int limit)
        {
            try
            {
                var titres = this._data.titres.OrderBy(t => t.Libelle).Skip(offset).Take(limit);
                this._logger.LogDebug("Recherche du titre {offset} - {limit}", offset, limit);

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche du titre {offset} - {limit}", offset, limit);
                throw;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Titre> FindAll()
        {
            try
            {
                this._logger.LogDebug("Récupération de tous les titres");
                var titres = this._data.titres;

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de tous les titres");
                throw;
            }
        }

        /// <inheritdoc/>
        public void IncrementNbLectures(Titre titre)
        {
            this._logger.LogWarning("Nouvelle lecture d'un titre dans le repository local. Opération non supportée.");
        }

        /// <inheritdoc/>
        public void IncrementNbLikes(Titre titre)
        {
            this._logger.LogWarning("Tentative de like d'un titre dans le repository local. Opération non supportée.");
        }

        /// <inheritdoc/>
        public IEnumerable<Titre> Search(string mot)
        {
            try
            {
                var titres = this._data.titres.Where(t => t.Libelle.Contains(mot, StringComparison.OrdinalIgnoreCase) || t.Chronique.Contains(mot, StringComparison.OrdinalIgnoreCase));
                this._logger.LogDebug("Recherche de titres avec le mot : {mot} - {nbTitres} résultats trouvés", mot, titres.Count());

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche de titre");
                throw;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Titre> SearchByStyle(string libelle)
        {
            try
            {
                var titres = this._data.titres
                                .Where(t => t.Styles.Any(s => s.Libelle.ToLower() == libelle.ToLower()));
                this._logger.LogInformation("Recherche de titres avec le style : {libelle} - {nbTitres} résultats trouvés", libelle, titres.Count());

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche par style");
                throw;
            }
        }

        /// <inheritdoc/>
        public void Update(Titre titre)
        {
            try
            {
                var titreExistant = _data.titres.FirstOrDefault(t => t.IdTitre == titre.IdTitre);
                if (titreExistant != null)
                {
                    titreExistant.Libelle = titre.Libelle;
                    titreExistant.Chronique = titre.Chronique;
                    titreExistant.DateSortie = titre.DateSortie;
                    titreExistant.NbLectures = titre.NbLectures;
                    titreExistant.NbLikes = titre.NbLikes;
                    titreExistant.IdArtiste = titre.IdArtiste;
                    titreExistant.Artiste = titre.Artiste;
                    titreExistant.Styles = titre.Styles;
                    titreExistant.Commentaires = titre.Commentaires;
                    this._logger.LogDebug($"Titre {titre.IdTitre} mis à jour dans le repository local.");
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Erreur lors de la mise à jour du titre {titre.IdTitre} dans le repository local.", ex);
                throw;
            }
        }
    }
}
