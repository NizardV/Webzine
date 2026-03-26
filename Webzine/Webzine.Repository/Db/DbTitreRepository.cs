using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Webzine.Repository.Db
{
    public class DbTitreRepository
    {
        private readonly DbContext _context;
        DbTitreRepository(DbContext context)
        {
            _context = context;
        }
    }
}
