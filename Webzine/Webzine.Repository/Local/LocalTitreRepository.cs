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
            var titres = _data.titres.Skip(offset).Take(limit).OrderBy(t => t.Libelle);
            return titres;
        }
        public IEnumerable<Titre> FindAll()
        {
            var titres = _data.titres;
            return titres;
        }
        public void IncrementNbLectures(Titre titre)
        {
            titre.NbLectures++;
        }
        public void IncrementNbLikes(Titre titre)
        {
            titre.NbLikes++;
        }
        public IEnumerable<Titre> Search(string mot)
        {
            var titres = _data.titres.Where(t => t.Libelle.Contains(mot, StringComparison.OrdinalIgnoreCase) || t.Chronique.Contains(mot, StringComparison.OrdinalIgnoreCase));
            return titres;
        }
        public IEnumerable<Titre> SearchByStyle(string libelle)
        {
            var titres = _data.titres.Where(t => t.Styles.Any(s => s.Libelle.Equals(libelle, StringComparison.OrdinalIgnoreCase)));
            return titres;
        }
        public void Update(Titre titre)
        {
            throw new NotImplementedException();
        }
    }
}
