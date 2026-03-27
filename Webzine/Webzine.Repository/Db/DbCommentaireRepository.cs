using Webzine.Entity;
using Webzine.EntitiesContext;
using Webzine.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Webzine.Repository.Db
{
    public class DbCommentaireRepository : ICommentaireRepository
    {
        private readonly WebzineDbContext _context;

        public DbCommentaireRepository(WebzineDbContext context)
        {
            _context = context;
        }

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