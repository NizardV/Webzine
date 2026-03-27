using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Logging;
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
            try
            {
                _logger.LogInformation($"Suppression du titre {titre.IdTitre} - {titre.Libelle}");
                _data.titres.Remove(titre);
            }
            catch (Exception)
            {
                _logger.LogError($"Erreur lors de la suppression du titre {titre.IdTitre} - {titre.Libelle}");
                throw new Exception("Impossible de supprimer le titre");
            }
        }
        public Titre Find(int idTitre)
        {
            try
            {
                var titre = _data.titres.FirstOrDefault(t => t.IdTitre == idTitre);
                _logger.LogInformation($"Recherche du titre {idTitre} - {titre.Libelle}");
                return titre;
            }
            catch (Exception)
            {
                _logger.LogError($"Erreur lors de la recherche du titre {idTitre}");
                throw new Exception("Titre non trouvé");
            }
        }
        public IEnumerable<Titre> FindTitres(int offset, int limit)
        {
            try
            {
                var titres = _data.titres.Skip(offset).Take(limit).OrderBy(t => t.Libelle);
                return titres;
            }
            catch (Exception)
            {

                throw new Exception("Impossible de retourner les titres demandés.");
            }
        }
        public IEnumerable<Titre> FindAll()
        {
            try
            {
                _logger.LogInformation("Récupération de tous les titres");
                var titres = _data.titres;
                return titres;
            }
            catch (Exception)
            {
                _logger.LogError("Erreur lors de la récupération de tous les titres");
                throw new Exception("Impossible de retourner les titres");
            }
        }
        public void IncrementNbLectures(Titre titre)
        {
            _logger.LogInformation($"Nouvelle lecture sur le titre : {titre.IdTitre} - {titre.Libelle}");
            titre.NbLectures++;
        }
        public void IncrementNbLikes(Titre titre)
        {
            try
            {
                titre.NbLikes++;
                _logger.LogInformation($"Nouveau like sur : {titre.IdTitre} - {titre.Libelle}");
            }
            catch (Exception)
            {
                _logger.LogError($"Erreur lors du like du titre {titre.IdTitre} - {titre.Libelle}");
                throw new Exception("Impossible de liker ce titre.s");
            }
        }
        public IEnumerable<Titre> Search(string mot)
        {
            try
            {
                var titres = _data.titres.Where(t => t.Libelle.Contains(mot, StringComparison.OrdinalIgnoreCase) || t.Chronique.Contains(mot, StringComparison.OrdinalIgnoreCase));
                _logger.LogInformation($"Recherche de titres avec le mot : {mot} - {titres.Count()} résultats trouvés");
                return titres;
            }
            catch (Exception)
            {

                throw new Exception("Impossible d'effectuer la recherche");
            }
        }
        public IEnumerable<Titre> SearchByStyle(string libelle)
        {
            try
            {
                var titres = _data.titres.Where(t => t.Styles.Any(s => s.Libelle.Equals(libelle, StringComparison.OrdinalIgnoreCase)));
                _logger.LogInformation($"Recherche de titres avec le style : {libelle} - {titres.Count()} résultats trouvés");
                return titres;
            }
            catch (Exception)
            {

                throw new Exception("Impossible de rechercher par style.");
            }
        }
        public void Update(Titre titre)
        {
            throw new NotImplementedException();
        }
    }
}
