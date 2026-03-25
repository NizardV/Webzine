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
        public LocalTitreRepository(WebzineData data)
        {
            _data = data;
        }

        public void Add(Titre titre)
        {
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
            return _data.titres.FirstOrDefault(t => t.IdTitre == idTitre);
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
        }
    }
}
