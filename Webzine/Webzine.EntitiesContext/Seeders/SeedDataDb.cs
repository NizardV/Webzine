using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;
using Webzine.Entity.Fixtures;

namespace Webzine.EntitiesContext.Seeders
{
    public class SeedDataDb
    {
        public static void Initialize(WebzineDbContext _context)
        {
            var styles = StyleFactory.CreerStyles(18);
            var artistes = ArtisteFactory.CreerArtistes(50);
            var titres = TitreFactory.CreerTitres(styles, artistes, 200);
            var commentaires = CommentaireFactory.CreerCommentaires(titres, 100);

            _context.Titres.AddRange(titres);
            _context.Styles.AddRange(styles);
            _context.Artistes.AddRange(artistes);
            _context.Commentaires.AddRange(commentaires);

            _context.SaveChanges();
        }
    }
}
