using System;
using System.Collections.Generic;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Db
{
    public class DbArtisteRepository : IArtisteRepository
    {
        public void Add(Artiste artiste)
        {
            throw new NotImplementedException();
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public void Delete(Artiste artiste)
        {
            throw new NotImplementedException();
        }

        public Artiste? Find(int idArtiste)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Artiste> FindAll()
        {
            throw new NotImplementedException();
        }

        public Artiste? FindByName(string nom)
        {
            throw new NotImplementedException();
        }

        public void Update(Artiste artiste)
        {
            throw new NotImplementedException();
        }
    }
}
