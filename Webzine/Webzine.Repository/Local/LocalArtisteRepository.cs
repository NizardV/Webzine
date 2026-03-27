using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    public class LocalArtisteRepository : IArtisteRepository
    {
        private readonly WebzineData data;
        private readonly ILogger<LocalArtisteRepository> logger;

        public LocalArtisteRepository(WebzineData data, ILogger<LocalArtisteRepository> logger)
        {
            this.data = data;
            this.logger = logger;
        }

        public void Add(Artiste artiste)
        {
            //if (artiste == null)
            //{
            //    this.logger.LogWarning("Tentative d'ajout d'un artiste null.");
            //    return;
            //}

            //if (artiste.IdArtiste <= 0)
            //{
            //    var nextId = this.data.artistes.Any()
            //        ? this.data.artistes.Max(a => a.IdArtiste) + 1
            //        : 1;
            //    artiste.IdArtiste = nextId;
            //}

            //this.data.artistes.Add(artiste);
            //this.logger.LogInformation("Artiste ajouté (Id: {ArtisteId}, Nom: {Nom}).", artiste.IdArtiste, artiste.Nom);
            throw new NotImplementedException();
        }

        public int Count()
        {
            var count = this.data.artistes.Count();
            this.logger.LogInformation("Comptage des artistes: {Count}.", count);
            return count;
        }

        public void Delete(Artiste artiste)
        {
            //if (artiste == null)
            //{
            //    this.logger.LogWarning("Tentative de suppression d'un artiste null.");
            //    return;
            //}

            //this.data.artistes.Remove(artiste);
            //this.logger.LogInformation("Artiste supprimé (Id: {ArtisteId}).", artiste.IdArtiste);
            throw new NotImplementedException();
        }

        public Artiste? Find(int idArtiste)
        {
            var artiste = this.data.artistes.FirstOrDefault(a => a.IdArtiste == idArtiste);
            this.logger.LogInformation("Recherche artiste par id {ArtisteId}. Trouvé: {Found}.", idArtiste, artiste != null);
            return artiste;
        }

        public IEnumerable<Artiste> FindAll()
        {
            this.logger.LogInformation("Récupération de tous les artistes.");
            return this.data.artistes;
        }

        public Artiste? FindByName(string nom)
        {
            if (string.IsNullOrWhiteSpace(nom))
            {
                this.logger.LogWarning("Recherche d'artiste avec un nom vide.");
                return null;
            }

            var artiste = this.data.artistes.FirstOrDefault(a => string.Equals(a.Nom, nom, StringComparison.OrdinalIgnoreCase));
            this.logger.LogInformation("Recherche artiste par nom {Nom}. Trouvé: {Found}.", nom, artiste != null);
            return artiste;
        }

        public void Update(Artiste artiste)
        {
            //if (artiste == null)
            //{
            //    this.logger.LogWarning("Tentative de mise à jour d'un artiste null.");
            //    return;
            //}

            //var existing = this.data.artistes.FirstOrDefault(a => a.IdArtiste == artiste.IdArtiste);
            //if (existing == null)
            //{
            //    this.logger.LogWarning("Artiste introuvable pour mise à jour (Id: {ArtisteId}).", artiste.IdArtiste);
            //    return;
            //}

            //existing.Nom = artiste.Nom;
            //existing.Biographie = artiste.Biographie;
            //this.logger.LogInformation("Artiste mis à jour (Id: {ArtisteId}).", artiste.IdArtiste);
            throw new NotImplementedException();
        }
    }
}
