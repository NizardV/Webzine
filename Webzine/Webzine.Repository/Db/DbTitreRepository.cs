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
            try
            {
                this._logger.LogDebug("Tentative d'ajout du titre {Libelle} dans le repository base de données.", titre.Libelle);

                this._context.Titres.Add(titre);
                this._context.SaveChanges();

                this._logger.LogDebug("Titre {Libelle} ajouté avec succès (ID: {IdTitre}).", titre.Libelle, titre.IdTitre);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de l'ajout du titre {Libelle} dans le repository base de données.", titre.Libelle);
                throw new Exception($"Impossible d'ajouter le titre '{titre.Libelle}' en base de données.", ex);
            }
        }
        public int Count()
        {
            try
            {
                this._logger.LogDebug("Comptage des titres en base de données.");
                var count = this._context.Titres.Count();
                this._logger.LogDebug("Comptage des titres: {Count}.", count);
                return count;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du comptage des titres en base de données.");
                throw new Exception("Impossible de compter les titres en base de données.", ex);
            }
        }
        public void Delete(Titre titre)
        {
            try
            {
                this._logger.LogDebug("Suppression du titre {IdTitre} - {Libelle}", titre.IdTitre, titre.Libelle);
                this._context.Titres.Remove(titre);
                this._context.SaveChanges();
                this._logger.LogDebug("Titre {IdTitre} supprimé avec succès.", titre.IdTitre);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la suppression du titre {IdTitre} - {Libelle}", titre.IdTitre, titre.Libelle);
                throw new Exception($"Impossible de supprimer le titre {titre.IdTitre} de la base de données.", ex);
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

                if (titre != null)
                {
                    this._logger.LogDebug("Recherche du titre {IdTitre} - {Libelle} trouvée", idTitre, titre.Libelle);
                }
                else
                {
                    this._logger.LogDebug("Recherche du titre {IdTitre} - Aucun résultat", idTitre);
                }

                return titre;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche du titre {IdTitre}", idTitre);
                throw new Exception($"Impossible de rechercher le titre avec l'ID {idTitre} en base de données.", ex);
            }
        }
        public IEnumerable<Titre> FindTitres(int offset, int limit)
        {
            try
            {
                this._logger.LogDebug("Récupération des titres avec offset {Offset} et limit {Limit} en base de données.", offset, limit);
                var titres = this._context.Titres.OrderBy(t => t.Libelle).Skip(offset).Take(limit).Include( t=> t.Artiste).ToList();
                this._logger.LogDebug("Récupération des titres avec offset {Offset} et limit {Limit}: {NbTitres} résultats trouvés.", offset, limit, titres.Count());

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération des titres avec offset {Offset} et limit {Limit}.", offset, limit);
                throw new Exception($"Impossible de récupérer les titres (offset: {offset}, limit: {limit}) en base de données.", ex);
            }
        }
        public IEnumerable<Titre> FindAll()
        {
            try
            {
                this._logger.LogDebug("Récupération de tous les titres");
                var titres = this._context.Titres
                    .OrderBy(t => t.Libelle)
                    .ThenBy(t => t.Artiste.Nom)
                    .Include(t => t.Artiste)
                    .Include(t => t.Styles)
                    .Include(t => t.Commentaires);

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de tous les titres");
                throw new Exception("Impossible de récupérer tous les titres en base de données.", ex);
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
                this._logger.LogError(ex, "Erreur lors de l'incrémentation d'une lecture d'un titre {IdTitre}", titre.IdTitre);
                throw new Exception($"Impossible d'incrémenter le nombre de lectures du titre {titre.IdTitre} en base de données.", ex);
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
                throw new Exception($"Impossible d'incrémenter le nombre de likes du titre {titre.IdTitre} en base de données.", ex);
            }
        }
        public IEnumerable<Titre> Search(string mot)
        {
            try
            {
                this._logger.LogDebug("Recherche de titres avec le mot {Mot} en base de données.", mot);
                var titres = this._context.Titres.Where(t =>
                t.Libelle.Contains(mot, StringComparison.OrdinalIgnoreCase) ||
                t.Chronique.Contains(mot, StringComparison.OrdinalIgnoreCase));
                this._logger.LogDebug("Recherche de titres avec le mot {Mot}: {NbTitres} résultats trouvés.", mot, titres.Count());

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche d'un titre avec le mot {Mot}.", mot);
                throw new Exception($"Impossible de rechercher les titres avec le mot '{mot}' en base de données.", ex);
            }
        }
        public IEnumerable<Titre> SearchByStyle(string libelle)
        {
            try
            {
                this._logger.LogDebug("Recherche de titres avec le style {Libelle} en base de données.", libelle);
                libelle = libelle?.ToLower() ?? "";
                var titres = this._context.Titres
                                .Where(t => t.Styles.Any(s => s.Libelle.ToLower() == libelle))
                                .Include(t => t.Styles)
                                .Include(t => t.Artiste);
                this._logger.LogDebug("Recherche de titres avec le style {Libelle}: {NbTitres} résultats trouvés.", libelle, titres.Count());

                return titres;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la recherche par style {Libelle}.", libelle);
                throw new Exception($"Impossible de rechercher les titres avec le style '{libelle}' en base de données.", ex);
            }
        }
        public void Update(Titre titre)
        {
            try
            {
                this._logger.LogDebug("Tentative de modification du titre {IdTitre} dans le repository base de données.", titre.IdTitre);

                this._context.Titres.Update(titre);
                this._context.SaveChanges();

                this._logger.LogDebug("Titre {IdTitre} modifié avec succès.", titre.IdTitre);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la modification du titre {IdTitre} dans le repository base de données.", titre.IdTitre);
                throw new Exception($"Impossible de mettre à jour le titre {titre.IdTitre} en base de données.", ex);
            }
        }

        public int SumLikes()
        {
            try
            {
                this._logger.LogDebug("Calcul du nombre total de likes sur tous les titres en base de données.");
                var totalLikes = this._context.Titres.Sum(t => t.NbLikes);
                this._logger.LogDebug("Nombre total de likes calculé: {TotalLikes}.", totalLikes);
                return totalLikes;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du calcul du nombre total de likes sur tous les titres en base de données.");
                throw new Exception("Impossible de calculer le nombre total de likes sur tous les titres en base de données.", ex);
            }
        }

        public int SumLectures()
        {
            try
            {
                this._logger.LogDebug("Calcul du nombre total de lectures sur tous les titres en base de données.");
                var totalLectures = this._context.Titres.Sum(t => t.NbLectures);
                this._logger.LogDebug("Nombre total de lectures calculé: {TotalLectures}.", totalLectures);
                return totalLectures;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du calcul du nombre total de lectures sur tous les titres en base de données.");
                throw new Exception("Impossible de calculer le nombre total de lectures sur tous les titres en base de données.", ex);
            }
        }

        public Titre GetTitrePlusLu()
        {
            try
            {
                this._logger.LogDebug("Récupération du titre le plus lu en base de données.");
                var titre = this._context.Titres.OrderByDescending(t => t.NbLectures).FirstOrDefault();

                if (titre != null)
                {
                    this._logger.LogDebug("Titre {IdTitre} - {Libelle} le plus lu trouvé.", titre.IdTitre, titre.Libelle);
                }
                else
                {
                    this._logger.LogDebug("Aucun titre trouvé en base de données.");
                }

                return titre;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération du titre le plus lu en base de données.");
                throw new Exception("Impossible de récupérer le titre le plus lu en base de données.", ex);
            }
        }
    }
}
