using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Business.Contracts.Services;
using Webzine.Repository.Contracts;
using Webzine.Entity;

namespace Webzine.Business.Services
{
    public class TitreService : ITitreService
    {
        private readonly ITitreRepository _titreRepository;
        public TitreService(ITitreRepository titreRepository)
        {
            _titreRepository = titreRepository;
        }
        public void Add(Titre titre)
        {
            try
            {
                _titreRepository.Add(titre);
            }
            catch (Exception)
            {

                throw new Exception("Impossible de créer le titre");
            } 
        }
        public int Count()
        {
            try
            {
                return _titreRepository.Count();
            }
            catch (Exception)
            {

                throw new Exception("Impossible de compter les titres");
            }
        }
        public void Delete(Entity.Titre titre)
        {
            try
            {
                _titreRepository.Delete(titre);
            }
            catch (Exception)
            {
                throw new Exception("Impossible de supprimer le titre");
            }
        }
        public Titre Find(int idTitre)
        {
            try
            {
                return _titreRepository.Find(idTitre);
            }
            catch (Exception)
            {
                throw new Exception("Impossible de trouver le titre");
            }
        }
        public IEnumerable<Titre> FindTitres(int offset, int limit)
        {
            try
            {
                return _titreRepository.FindTitres(offset, limit);
            }
            catch (Exception)
            {
                throw new Exception("Impossible de trouver les titres");
            }
        }
        public IEnumerable<Titre> FindAll()
        {
            try
            {
                return _titreRepository.FindAll();
            }
            catch (Exception)
            {
                throw new Exception("Impossible de trouver les titres");
            }
        }
        public void IncrementNbLectures(Titre titre)
        {
            try
            {
                _titreRepository.IncrementNbLectures(titre);
            }
            catch (Exception)
            {
                throw new Exception("Impossible d'incrémenter le nombre de lectures du titre");
            }
        }
        public void IncrementNbLikes(Titre titre)
        {
            try
            {
                _titreRepository.IncrementNbLikes(titre);
            }
            catch (Exception)
            {
                throw new Exception("Impossible d'incrémenter le nombre de likes du titre");
            }
        }
        public IEnumerable<Titre> Search(string mot)
        {
            try
            {
                return _titreRepository.Search(mot);
            }
            catch (Exception)
            {
                throw new Exception("Impossible de trouver les titres");
            }
        }
        public IEnumerable<Titre> SearchByStyle(string libelle)
        {
            try
            {
                return _titreRepository.SearchByStyle(libelle);
            }
            catch (Exception)
            {
                throw new Exception("Impossible de trouver les titres");
            }
        }
        public void Update(Titre titre)
        {
            try
            {
                _titreRepository.Update(titre);
            }
            catch (Exception)
            {
                throw new Exception("Impossible de mettre à jour le titre");
            }
        }
    }
}
