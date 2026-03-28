using Webzine.Entity;

namespace Webzine.Repository.Contracts
{
    public interface IArtisteRepository
    {
        void Add(Artiste artiste);
        int Count();
        void Delete(Artiste artiste);
        Artiste? Find(int idArtiste);
        IEnumerable<Artiste> FindAll();
        Artiste? FindByName(string nom);
        void Update(Artiste artiste);
    }
}
