using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;

namespace Webzine.Repository.Db
{
    public class DbStyleRepository
    {
        private readonly DbContext _context;

        DbStyleRepository(DbContext context)
        {
            _context = context;
        }

        public void Add(Style style)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }

        public void Delete(Style style)
        {
            _context.styles.Remove(style);
            _context.SaveChanges();
        }

        public Style Find(int id)
        {
            return _context.styles.FirstOrDefault(s => s.IdStyle == id);
        }

        public IEnumerable<Style> FindAll()
        {
            var styles = _context.styles;
            return styles;
        }

        public void Update(Style style)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }
    }
}
