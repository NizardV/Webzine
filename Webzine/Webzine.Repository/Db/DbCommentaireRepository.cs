using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository.Db
{
    public class DbCommentaireRepository : ICommentaireRepository
    {
        private readonly WebzineDbContext _context;

        public void Add(Commentaire commentaire)
        {
            _context.Commentaires.Add(commentaire);
            _context.SaveChanges();
        }

        public void Delete(Commentaire commentaire)
        {
            _context.Commentaires.Remove(commentaire);
            _context.SaveChanges();
        }

        public Commentaire Find(int id)
        {
            return _context.Commentaires.Include(c => c.Titre).FirstOrDefault(c => c.IdCommentaire == id);
        }

        public IEnumerable<Commentaire> FindAll()
        {
            return _context.Commentaires.Include(c => c.Titre).ToList();
        }
    }
}