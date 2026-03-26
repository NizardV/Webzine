using System;
using System.Collections.Generic;
using System.Linq;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Local
{
    public class LocalArtisteRepository : IArtisteRepository
    {
        private readonly WebzineData data;

        public LocalArtisteRepository(WebzineData data)
        {
            this.data = data;
        }

        public void Add(Artiste artiste)
        {
            if (artiste == null)
            {
                return;
            }

            this.data.artistes.Add(artiste);
        }

        public int Count()
        {
            return this.data.artistes.Count();
        }

        public void Delete(Artiste artiste)
        {
            if (artiste == null)
            {
                return;
            }

            this.data.artistes.Remove(artiste);
        }

        public Artiste? Find(int idArtiste)
        {
            return this.data.artistes.FirstOrDefault(a => a.IdArtiste == idArtiste);
        }

        public IEnumerable<Artiste> FindAll()
        {
            return this.data.artistes;
        }

        public Artiste? FindByName(string nom)
        {
            if (string.IsNullOrWhiteSpace(nom))
            {
                return null;
            }

            return this.data.artistes.FirstOrDefault(a => string.Equals(a.Nom, nom, StringComparison.OrdinalIgnoreCase));
        }

        public void Update(Artiste artiste)
        {
            if (artiste == null)
            {
                return;
            }

            var existing = this.data.artistes.FirstOrDefault(a => a.IdArtiste == artiste.IdArtiste);
            if (existing == null)
            {
                return;
            }

            existing.Nom = artiste.Nom;
            existing.Biographie = artiste.Biographie;
        }
    }
}
