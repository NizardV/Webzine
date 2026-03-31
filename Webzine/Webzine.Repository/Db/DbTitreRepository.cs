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
    /// <inheritdoc/>
    public class DbTitreRepository : ITitreRepository
    {
        private readonly WebzineDbContext _context;
        private readonly ILogger<DbTitreRepository> _logger;
        public DbTitreRepository(WebzineDbContext context, ILogger<DbTitreRepository> logger)
        {
            this._context = context;
            this._logger = logger;
        }

        public void Add(Titre titre)
        {
            this._logger.LogWarning("Tentative d'ajout du style {titre}. Méthode non encore implémentée dans le repository base de données.", titre);
            throw new NotImplementedException();
        }
        public int Count()
        {
            try
            {
                return this._context.Titres.Count();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du comptage des titres en base de données.");
                throw;
            }
        }
        public void Delete(Titre titre)
        {
            try
            {
                this._logger.LogDebug("Suppression du titre {IdTitre} - {Libelle}", titre.IdTitre, titre.Libelle);
                this._context.Titres.Remove(titre);
                this._context.SaveChanges();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la suppression du titre {IdTitre} - {Libelle}", titre.IdTitre, titre.Libelle);
                throw;
            }
        }
        public Titre Find(int idTitre)
        {
            try
            {
                var titre = this._context.Titres
                    .Include(t => t.Artiste)
                    .Include(t => t.Commentaires)
                    .Include(t => t.Styles)
                    .FirstOrDefault(t => t.IdTitre == idTitre);
                this._logger.LogDebug("Recherche du titre {idTitre} - {Libelle}", idTitre, titre.Libelle);

                return titre;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche du titre {idTitre}", idTitre);
                throw;
            }
        }
        public IEnumerable<Titre> FindTitres(int offset, int limit)
        {
            try
            {
                var titres = this._context.Titres.Skip(offset).Take(limit).OrderBy(t => t.Libelle);
                this._logger.LogDebug("Récupération des titres avec offset {offset} et limit {limit} - {nbTitres} résultats trouvés", offset, limit, titres.Count());

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération des titres avec offset {offset} et limit {limit}", offset, limit);
                throw;
            }
        }
        public IEnumerable<Titre> FindAll()
        {
            try
            {
                this._logger.LogDebug("Récupération de tous les titres");
                var titres = this._context.Titres
                    .Include(t => t.Artiste)
                    .Include(t => t.Styles)
                    .Include(t => t.Commentaires);

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de tous les titres");
                throw;
            }
        }
        public void IncrementNbLectures(Titre titre)
        {
            try
            {
                var titreBDD = this._context.Titres.Where(t => t.IdTitre == titre.IdTitre).FirstOrDefault();
                titreBDD.NbLectures++;
                this._context.SaveChanges();
                this._logger.LogDebug("Nouvelle lecture sur le titre : {IdTitre} - {Libelle}", titre.IdTitre, titre.Libelle);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la l'incrémentation d'une lecture d'un titre");
                throw;
            }
        }
        public void IncrementNbLikes(Titre titre)
        {
            try
            {
                var titreBDD = this._context.Titres.Where(t => t.IdTitre == titre.IdTitre).FirstOrDefault();
                titreBDD.NbLikes++;
                this._context.SaveChanges();
                this._logger.LogDebug("Nouveau like sur : {IdTitre} - {Libelle}", titre.IdTitre, titre.Libelle);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du like du titre {IdTitre} - {Libelle}", titre.IdTitre, titre.Libelle);
                throw;
            }
        }
        public IEnumerable<Titre> Search(string mot)
        {
            try
            {
                var titres = this._context.Titres.Where(t =>
                t.Libelle.Contains(mot, StringComparison.OrdinalIgnoreCase) ||
                t.Chronique.Contains(mot, StringComparison.OrdinalIgnoreCase));
                this._logger.LogDebug("Recherche de titres avec le mot : {mot} - {nbTitres} résultats trouvés", mot, titres.Count());

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche d'un titre {mot}", mot);
                throw;
            }
        }
        public IEnumerable<Titre> SearchByStyle(string libelle)
        {
            try
            {
                libelle = libelle?.ToLower() ?? "";
                var titres = this._context.Titres
                                .Where(t => t.Styles.Any(s => s.Libelle.ToLower() == libelle))
                                .Include(t => t.Styles)
                                .Include(t => t.Artiste);
                this._logger.LogDebug("Recherche de titres avec le style : {libelle} - {nbTitres} résultats trouvés", libelle, titres.Count());

                if (!titres.Any())
                {
                    this._logger.LogWarning("Aucun titre trouvé pour le style : {libelle}", libelle);
                }

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche par style {libelle}", libelle);
                throw;
            }
        }
        public void Update(Titre titre)
        {
            this._logger.LogWarning("Tentative de modification du style {titre}. Méthode non encore implémentée dans le repository base de données.", titre);
            throw new NotImplementedException();
        }

        public int SumLikes()
        {
            try
            {
                return this._context.Titres.Sum(t => t.NbLikes);

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du calcul du nombre total de likes sur tous les titres");
                throw;
            }
        }

        public int SumLectures()
        {
            try
            {
                return this._context.Titres.Sum(t => t.NbLectures);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du calcul du nombre total de lectures sur tous les titres");
                throw;
            }
        }

        public Titre GetTitrePlusLu()
        {
            try
            {
                 return this._context.Titres.OrderByDescending(t => t.NbLectures).FirstOrDefault();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération du titre le plus lu");
                throw;
            }
        }
    }
}
