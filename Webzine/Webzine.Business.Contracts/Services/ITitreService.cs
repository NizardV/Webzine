using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;
using Webzine.Entity;

namespace Webzine.Business.Contracts.Services
{
    public interface ITitreService
    {
        void Add(Titre titre);
        int Count();
        void Delete(Titre titre);
        Titre Find(int idTitre);
        IEnumerable<Titre> FindTitres(int offset, int limit);
        IEnumerable<Titre> FindAll();
        void IncrementNbLectures(Titre titre);
        void IncrementNbLikes(Titre titre);
        IEnumerable<Titre> Search(string mot);
        IEnumerable<Titre> SearchByStyle(string libelle);
        void Update(Titre titre);
    }
}
