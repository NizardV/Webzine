using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    /// <inheritdoc/>
    public class LocalArtisteRepository : IArtisteRepository
    {
        private readonly WebzineData _data;
        private readonly ILogger<LocalArtisteRepository> _logger;

        /// <inheritdoc/>
        public LocalArtisteRepository(WebzineData data, ILogger<LocalArtisteRepository> logger)
        {
            this._data = data;
            this._logger = logger;
        }

        /// <inheritdoc/>
        public void Add(Artiste artiste)
        {
            try
            {
                this._logger.LogDebug(
                    "Tentative d'ajout de l'artiste {Nom} dans le repository local.",
                    artiste.Nom);

                this._data.artistes.Add(artiste);

                this._logger.LogDebug(
                    "Artiste {Nom} ajouté avec succès au repository local.",
                    artiste.Nom);
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de l'ajout de l'artiste {Nom} au repository local.",
                    artiste.Nom);
                throw;
            }
        }

        /// <inheritdoc/>
        public int Count()
        {
            var count = this._data.artistes.Count();
            this._logger.LogDebug("Comptage des artistes: {Count}.", count);
            return count;
        }

        /// <inheritdoc/>
        public void Delete(Artiste artiste)
        {
            try
            {
                this._logger.LogDebug(
                    "Tentative de suppression de l'artiste {IdArtiste} du repository local.",
                    artiste.IdArtiste);

                var artisteASupprimer = this._data.artistes.FirstOrDefault(a => a.IdArtiste == artiste.IdArtiste);
                if (artisteASupprimer != null)
                {
                    this._data.artistes.Remove(artisteASupprimer);
                    this._logger.LogDebug(
                        "Artiste {IdArtiste} supprimé avec succès du repository local.",
                        artiste.IdArtiste);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la suppression de l'artiste {IdArtiste} du repository local.",
                    artiste.IdArtiste);
                throw;
            }
        }

        /// <inheritdoc/>
        public Artiste? Find(int idArtiste)
        {
            try
            {
                this._logger.LogDebug(
                    "Recherche du style d'identifiant {IdStyle} dans le repository local.",
                    idArtiste);

                var artiste = this._data.artistes.FirstOrDefault(a => a.IdArtiste == idArtiste);
                this._logger.LogDebug("Recherche artiste par id {ArtisteId}. Trouvé: {Found}.", idArtiste, artiste != null);
                return artiste;
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la recherche de l'artiste {idArtiste} dans le repository local.", idArtiste);
                throw;
            }
        }

        /// <inheritdoc/>
        public IEnumerable<Artiste> FindAll()
        {
            try
            {
                this._logger.LogDebug(
                    "Récupération de tous les artistes depuis le repository local.");

                return this._data.artistes.ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la récupération de tous les artistes depuis le repository local.");
                throw;
            }
        }
        public IEnumerable<Artiste> FindArtistes(int offset, int limit)
        {
            try
            {
                this._logger.LogDebug("Récupération de tous les artistes en base.");

                return this._data.artistes
                    .OrderBy(a => a.Nom)
                    .Skip(offset)
                    .Take(limit)
                    .ToList();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de tous les artistes en base.");
                throw;
            }
        }


        /// <inheritdoc/>
        public Artiste? FindByName(string nom)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nom))
                {
                    this._logger.LogWarning("Recherche d'artiste avec un nom vide.");
                    return null;
                }

                var artiste = this._data.artistes.FirstOrDefault(a => string.Equals(a.Nom, nom, StringComparison.OrdinalIgnoreCase));
                this._logger.LogDebug("Recherche artiste par nom {Nom}. Trouvé: {Found}.", nom, artiste != null);

                return artiste;
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la recherche de l'artiste par nom {nom} dans le repository local.", nom);
                throw;
            }
        }

        /// <inheritdoc/>
        public void Update(Artiste artiste)
        {
            try
            {
                this._logger.LogDebug(
                    "Tentative de mise à jour de l'artiste {IdArtiste} dans le repository local.",
                    artiste.IdArtiste);

                var artisteExistant = this._data.artistes.FirstOrDefault(a => a.IdArtiste == artiste.IdArtiste);
                if (artisteExistant != null)
                {
                    artisteExistant.Nom = artiste.Nom;
                    artisteExistant.Biographie = artiste.Biographie;
                    this._logger.LogDebug(
                        "Artiste {IdArtiste} mis à jour avec succès dans le repository local.",
                        artiste.IdArtiste);
                }
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors de la mise à jour de l'artiste {IdArtiste} dans le repository local.",
                    artiste.IdArtiste);
                throw;
            }
        }

        /// <inheritdoc/>
        public int CountWithBiography()
        {
            try
            {
                return this._data.artistes.Count(a => a.Biographie != null && a.Biographie != "");
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors du comptage des artistes avec biographie en base.");
                throw;
            }
        }

        /// <inheritdoc/>
        public Artiste GetArtistePlusChronique()
        {
            try
            {
                return this._data.artistes
                       .OrderByDescending(a => a.Titres.Count(t => t.Chronique != null && t.Chronique != ""))
                       .FirstOrDefault();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de l'artiste ayant le plus de chroniques.");
                throw;
            }
        }


        /// <inheritdoc/>
        public Artiste GetArtistePlusAlbums()
        {
            try
            {
                return this._data.artistes
                       .OrderByDescending(a => a.Titres.Select(t => t.Album).Distinct().Count())
                       .FirstOrDefault();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "Erreur lors de la récupération de l'artiste ayant le plus d'albums.");
                throw;
            }
        }

    }
}
