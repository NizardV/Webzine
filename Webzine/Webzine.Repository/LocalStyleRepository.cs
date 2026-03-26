using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;
using Webzine.Repository.Contracts;

namespace Webzine.Repository
{
    public class LocalStyleRepository : IStyleRepository
    {
        public readonly WebzineData _data;

        public LocalStyleRepository(WebzineData data)
        {
            _data = data;
        }

        public void Add(Style style)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }

        public void Delete(Style style)
        {
            _data.styles.Remove(style);
        }

        public Style Find(int id)
        {
            return _data.styles.FirstOrDefault(s => s.IdStyle == id);
        }

        public IEnumerable<Style> FindAll()
        {
            var styles = _data.styles;
            return styles;
        }

        public void Update(Style style)
        {
            // Not developed yet.
            throw new NotImplementedException();
        }
    }
}
