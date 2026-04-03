using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
                this._logger.LogDebug("Tentative d'ajout du titre {Libelle} au repository local.", titre.Libelle);

                _data.titres.Add(titre);

                this._logger.LogDebug("Titre {Libelle} ajouté avec succès au repository local.", titre.Libelle);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de l'ajout du titre {Libelle} au repository local.", titre.Libelle);
                throw new Exception($"Impossible d'ajouter le titre '{titre.Libelle}' au repository local.", ex);
            }
        }

        public int Count()
        {
            try
            {
                this._logger.LogDebug("Comptage des titres dans le repository local.");
                var count = this._data.titres.Count();
                this._logger.LogDebug("Comptage des titres: {Count}.", count);
                return count;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du comptage des titres au repository local.");
                throw new Exception("Impossible de compter les titres du repository local.", ex);
            }
        }

        /// <inheritdoc/>
        public void Delete(Titre titre)
        {
            try
            {
                this._logger.LogDebug("Tentative de suppression du titre {IdTitre} du repository local.", titre.IdTitre);

                var titreASupprimer = _data.titres.FirstOrDefault(t => t.IdTitre == titre.IdTitre);
                if (titreASupprimer != null)
                {
                    _data.titres.Remove(titreASupprimer);
                    this._logger.LogDebug("Titre {IdTitre} supprimé avec succès du repository local.", titre.IdTitre);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la suppression du titre {IdTitre} du repository local.", titre.IdTitre);
                throw new Exception($"Impossible de supprimer le titre {titre.IdTitre} du repository local.", ex);
            }
        }

        /// <inheritdoc/>
        public Titre Find(int idTitre)
        {
            try
            {
                this._logger.LogDebug("Recherche du titre {IdTitre} dans le repository local.", idTitre);

                var titre = this._data.titres.FirstOrDefault(t => t.IdTitre == idTitre);

                if (titre != null)
                {
                    this._logger.LogDebug("Titre {IdTitre} - {Libelle} trouvé.", idTitre, titre.Libelle);
                }
                else
                {
                    this._logger.LogDebug("Aucun titre trouvé pour l'identifiant {IdTitre}.", idTitre);
                }

                return titre;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche du titre {IdTitre}.", idTitre);
                throw new Exception($"Impossible de rechercher le titre avec l'ID {idTitre} dans le repository local.", ex);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Titre> FindTitres(int offset, int limit)
        {
            try
            {
                this._logger.LogDebug("Recherche de titres avec offset {Offset} et limit {Limit}.", offset, limit);

                var titres = this._data.titres.OrderBy(t => t.Libelle).Skip(offset).Take(limit).ToList();

                this._logger.LogDebug("Récupération de {NbTitres} résultats trouvés.", titres.Count);

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche de titres avec offset {Offset} et limit {Limit}.", offset, limit);
                throw new Exception($"Impossible de récupérer les titres (offset: {offset}, limit: {limit}) du repository local.", ex);
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
                throw new Exception("Impossible de récupérer tous les titres du repository local.", ex);
            }
        }

        /// <inheritdoc/>
        public void IncrementNbLectures(Titre titre)
        {
            try
            {
                var titreEnMemoire = this._data.titres.FirstOrDefault(t => t.IdTitre == titre.IdTitre);
                if (titreEnMemoire != null)
                {
                    titreEnMemoire.NbLectures++;
                    this._logger.LogDebug("Nouvelle lecture d'un titre au repository local - {IdTitre}.", titre.IdTitre);
                }
                else
                {
                    this._logger.LogWarning("Tentative d'incrémentation du nombre de lectures d'un titre inexistant - {IdTitre}.", titre.IdTitre);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de l'incrémentation du nombre de lectures d'un titre au repository local - {IdTitre}.", titre.IdTitre);
                throw new Exception($"Impossible d'incrémenter le nombre de lectures du titre {titre.IdTitre} au repository local.", ex);
            }
        }

        /// <inheritdoc/>
        public void IncrementNbLikes(Titre titre)
        {
            try
            {
                var titreEnMemoire = this._data.titres.FirstOrDefault(t => t.IdTitre == titre.IdTitre);
                if (titreEnMemoire != null)
                {
                    titreEnMemoire.NbLikes++;
                    this._logger.LogDebug("Nouveau like d'un titre au repository local - {IdTitre}.", titre.IdTitre);
                }
                else
                {
                    this._logger.LogWarning("Tentative d'incrémentation du nombre de likes d'un titre inexistant - {IdTitre}.", titre.IdTitre);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de l'incrémentation du nombre de likes d'un titre au repository local - {IdTitre}.", titre.IdTitre);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Titre> Search(string mot)
        {
            try
            {
                this._logger.LogDebug("Recherche de titres avec le mot {Mot} dans le repository local.", mot);
                var titres = this._data.titres.Where(t => t.Libelle.Contains(mot, StringComparison.OrdinalIgnoreCase) || t.Chronique.Contains(mot, StringComparison.OrdinalIgnoreCase));
                this._logger.LogDebug("Recherche de titres avec le mot {Mot}: {NbTitres} résultats trouvés.", mot, titres.Count());

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche de titres avec le mot {Mot} dans le repository local.", mot);
                throw new Exception($"Impossible de rechercher les titres avec le mot '{mot}' dans le repository local.", ex);
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Titre> SearchByStyle(string libelle)
        {
            try
            {
                this._logger.LogDebug("Recherche de titres avec le style {Libelle} dans le repository local.", libelle);
                var titres = this._data.titres
                                .Where(t => t.Styles.Any(s => s.Libelle.ToLower() == libelle.ToLower()));
                this._logger.LogDebug("Recherche de titres avec le style {Libelle}: {NbTitres} résultats trouvés.", libelle, titres.Count());

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche de titres avec le style {Libelle} dans le repository local.", libelle);
                throw new Exception($"Impossible de rechercher les titres avec le style '{libelle}' dans le repository local.", ex);
            }
        }

        /// <inheritdoc/>
        public void Update(Titre titre)
        {
            try
            {
                this._logger.LogDebug("Tentative de mise à jour du titre {IdTitre} au repository local.", titre.IdTitre);

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
                    this._logger.LogDebug("Titre {IdTitre} mis à jour avec succès au repository local.", titre.IdTitre);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la mise à jour du titre {IdTitre} au repository local.", titre.IdTitre);
                throw new Exception($"Impossible de mettre à jour le titre {titre.IdTitre} dans le repository local.", ex);
            }
        }

        public int SumLikes()
        {
            try
            {
                this._logger.LogDebug("Calcul du nombre total de likes sur tous les titres dans le repository local.");
                var totalLikes = this._data.titres.Sum(t => t.NbLikes);
                this._logger.LogDebug("Nombre total de likes calculé: {TotalLikes}.", totalLikes);
                return totalLikes;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du calcul du nombre total de likes sur tous les titres dans le repository local.");
                throw new Exception("Impossible de calculer le nombre total de likes au repository local.", ex);
            }
        }

        public int SumLectures()
        {
            try
            {
                this._logger.LogDebug("Calcul du nombre total de lectures sur tous les titres dans le repository local.");
                var totalLectures = this._data.titres.Sum(t => t.NbLectures);
                this._logger.LogDebug("Nombre total de lectures calculé: {TotalLectures}.", totalLectures);
                return totalLectures;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du calcul du nombre total de lectures sur tous les titres dans le repository local.");
                throw new Exception("Impossible de calculer le nombre total de lectures au repository local.", ex);
            }
        }

        public Titre GetTitrePlusLu()
        {
            try
            {
                this._logger.LogDebug("Récupération du titre le plus lu dans le repository local.");
                var titre = this._data.titres.OrderByDescending(t => t.NbLectures).FirstOrDefault();

                if (titre != null)
                {
                    this._logger.LogDebug("Titre {IdTitre} - {Libelle} le plus lu trouvé.", titre.IdTitre, titre.Libelle);
                }
                else
                {
                    this._logger.LogDebug("Aucun titre trouvé dans le repository local.");
                }

                return titre;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération du titre le plus lu dans le repository local.");
                throw new Exception("Impossible de récupérer le titre le plus lu du repository local.", ex);
            }
        }
    }
}
