using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    public class LocalTitreRepository : ITitreRepository
    {
        public readonly WebzineData _data;
        private readonly ILogger<LocalTitreRepository> _logger;
        public LocalTitreRepository(WebzineData data, ILogger<LocalTitreRepository> logger)
        {
            _data = data;
            _logger = logger;
        }

        public void Add(Titre titre)
        {
            throw new NotImplementedException();
        }
        public int Count()
        {
            return _data.titres.Count();
        }
        public void Delete(Titre titre)
        {
            _data.titres.Remove(titre);
        }
        public Titre Find(int idTitre)
        {
            try
            {
                var titre = _data.titres.FirstOrDefault(t => t.IdTitre == idTitre);
                this._logger.LogDebug($"Recherche du titre {idTitre} - {titre.Libelle}");
                return titre;
            }
            catch (Exception)
            {
                this._logger.LogError($"Erreur lors de la recherche du titre {idTitre}");
                throw new Exception("Titre non trouvé");
            }
        }
        public IEnumerable<Titre> FindTitres(int offset, int limit)
        {
            try
            {
                var titres = _data.titres.Skip(offset).Take(limit).OrderBy(t => t.Libelle);
                this._logger.LogDebug($"Recherche du titre {offset} - {limit}");
                return titres;
            }
            catch (Exception)
            {
                this._logger.LogError($"Erreur lors de la recherche du titre {offset} - {limit}");
                throw new Exception("Impossible de retourner les titres demandés.");
            }
        }
        public IEnumerable<Titre> FindAll()
        {
            try
            {
                this._logger.LogDebug("Récupération de tous les titres");
                var titres = _data.titres;
                return titres;
            }
            catch (Exception)
            {
                this._logger.LogError("Erreur lors de la récupération de tous les titres");
                throw new Exception("Impossible de retourner les titres");
            }
        }
        public void IncrementNbLectures(Titre titre)
        {
            this._logger.LogWarning("Nouvelle lecture d'un titre dans le repository local. Opération non supportée.");
        }
        public void IncrementNbLikes(Titre titre)
        {
            this._logger.LogWarning("Tentative de like d'un titre dans le repository local. Opération non supportée.");
        }
        public IEnumerable<Titre> Search(string mot)
        {
            try
            {
                var titres = _data.titres.Where(t => t.Libelle.Contains(mot, StringComparison.OrdinalIgnoreCase) || t.Chronique.Contains(mot, StringComparison.OrdinalIgnoreCase));
                this._logger.LogDebug($"Recherche de titres avec le mot : {mot} - {titres.Count()} résultats trouvés");
                return titres;
            }
            catch (Exception)
            {
                this._logger.LogError("Erreur lors de la recherche de titre");
                throw new Exception("Impossible d'effectuer la recherche");
            }
        }
        public IEnumerable<Titre> SearchByStyle(string libelle)
        {
            try
            {
                var titres = _data.titres.Where(t => t.Styles.Any(s => s.Libelle.ToLower() == libelle));
                this._logger.LogDebug($"Recherche de titres avec le style : {libelle} - {titres.Count()} résultats trouvés");
                return titres;
            }
            catch (Exception)
            {
                this._logger.LogError("Erreur lors de la recherche par style");
                throw new Exception("Impossible de rechercher par style.");
            }
        }
        }
        public void Update(Titre titre)
        {
            this._logger.LogWarning("Tentative de de mise à jour d'un titre dans le repository local. Opération non supportée.");
        }
    }
}
