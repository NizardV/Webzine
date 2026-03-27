using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Webzine.EntitiesContext;
using Webzine.Entity;
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
            this._logger.LogWarning($"Tentative d'ajout du style {titre}. Méthode non encore implémentée dans le repository base de données.");
            throw new NotImplementedException();
        }
        public int Count()
        {
            try
            {
                return _context.Titres.Count();
            }
            catch (Exception ex)
            {
                this._logger.LogError("Erreur lors du comptage des titres en base de données.", ex);
                throw new Exception("Impossible de compter les titres");
            }
        }
        public void Delete(Titre titre)
        {
            try
            {
                this._logger.LogDebug($"Suppression du titre {titre.IdTitre} - {titre.Libelle}");
                _context.Titres.Remove(titre);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Erreur lors de la suppression du titre {titre.IdTitre} - {titre.Libelle}", ex);
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
                    .Include(t=> t.Styles)
                    .FirstOrDefault(t => t.IdTitre == idTitre);
                this._logger.LogDebug($"Recherche du titre {idTitre} - {titre.Libelle}");

                return titre;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Erreur lors de la recherche du titre {idTitre}", ex);
                throw new Exception("Titre non trouvé");
            }
        }
        public IEnumerable<Titre> FindTitres(int offset, int limit)
        {
            try
            {
                var titres = _context.Titres.Skip(offset).Take(limit).OrderBy(t => t.Libelle);
                this._logger.LogDebug($"Récupération des titres avec offset {offset} et limit {limit} - {titres.Count()} résultats trouvés");

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Erreur lors de la récupération des titres avec offset {offset} et limit {limit}", ex);
                throw new Exception("Impossible de retourner les titres demandés.");
            }
        }
        public IEnumerable<Titre> FindAll()
        {
            try
            {
                this._logger.LogDebug("Récupération de tous les titres");
                var titres = _context.Titres
                    .Include(t => t.Artiste)
                    .Include(t => t.Styles)
                    .Include(t => t.Commentaires);
                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError("Erreur lors de la récupération de tous les titres", ex);
                throw new Exception("Impossible de retourner les titres");
            }
        }
        public void IncrementNbLectures(Titre titre)
        {
            try
            {
                var titreBDD = _context.Titres.Where(t => t.IdTitre == titre.IdTitre).FirstOrDefault();
                titreBDD.NbLectures++;
                _context.SaveChanges();
                this._logger.LogDebug($"Nouvelle lecture sur le titre : {titre.IdTitre} - {titre.Libelle}");
            }
            catch (Exception ex)
            {
                this._logger.LogError("Erreur lors de la l'incrémentation d'une lecture d'un titre", ex);
                throw new Exception("Impossible d'incrémenter le nombre de lectures");
            }
        }
        public void IncrementNbLikes(Titre titre)
        {
            try
            {
                var titreBDD = _context.Titres.Where(t => t.IdTitre == titre.IdTitre).FirstOrDefault();
                titreBDD.NbLikes++;
                _context.SaveChanges();
                this._logger.LogDebug($"Nouveau like sur : {titre.IdTitre} - {titre.Libelle}");
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Erreur lors du like du titre {titre.IdTitre} - {titre.Libelle}", ex);
                throw new Exception("Impossible de liker ce titre.s");
            }
        }
        public IEnumerable<Titre> Search(string mot)
        {
            try
            {
                var titres = _context.Titres.Where(t => t.Libelle.Contains(mot, StringComparison.OrdinalIgnoreCase) || t.Chronique.Contains(mot, StringComparison.OrdinalIgnoreCase));
                this._logger.LogDebug($"Recherche de titres avec le mot : {mot} - {titres.Count()} résultats trouvés");
                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Erreur lors de la recherche d'un titre {mot}", ex);
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
                this._logger.LogDebug($"Recherche de titres avec le style : {libelle} - {titres.Count()} résultats trouvés");
                if (!titres.Any())
                {
                    this._logger.LogWarning($"Aucun titre trouvé pour le style : {libelle}");
                }
                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError($"Erreur lors de la recherche par style {libelle}", ex);
                throw new Exception("Impossible de rechercher par style.");
            }
        }
        public void Update(Titre titre)
        {
            this._logger.LogWarning($"Tentative de modification du style {titre}. Méthode non encore implémentée dans le repository base de données.");
            throw new NotImplementedException();
        }
    }
}
