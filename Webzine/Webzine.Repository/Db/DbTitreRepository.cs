using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;
using Webzine.EntitiesContext;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Db
{
    public class DbTitreRepository : ITitreRepository
    {
        private readonly WebzineDbContext _context;
        private readonly ILogger<DbTitreRepository> _logger;
        public DbTitreRepository(WebzineDbContext context, ILogger<DbTitreRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Add(Titre titre)
        {
            throw new NotImplementedException();
        }
        public int Count()
        {
            return _context.Titres.Count();
        }
        public void Delete(Titre titre)
        {
            try
            {
                _logger.LogInformation($"Suppression du titre {titre.IdTitre} - {titre.Libelle}");
                _context.Titres.Remove(titre);
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
                var titre = _context.Titres
                    .Include(t=> t.Artiste)
                    .Include(t=> t.Commentaires)
                    .FirstOrDefault(t => t.IdTitre == idTitre);
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
                var titres = _context.Titres.Skip(offset).Take(limit).OrderBy(t => t.Libelle);
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
                var titres = _context.Titres;
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
                var titres = _context.Titres.Where(t => t.Libelle.Contains(mot, StringComparison.OrdinalIgnoreCase) || t.Chronique.Contains(mot, StringComparison.OrdinalIgnoreCase));
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
                libelle = libelle?.ToLower() ?? "";
                var titres = _context.Titres
                                .Where(t => t.Styles.Any(s => s.Libelle.ToLower() == libelle))
                                .Include(t => t.Styles)
                                .Include(t => t.Artiste);
                _logger.LogInformation($"Recherche de titres avec le style : {libelle} - {titres.Count()} résultats trouvés");
                if (!titres.Any())
                {
                    _logger.LogWarning($"Aucun titre trouvé pour le style : {libelle}");
                }
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
