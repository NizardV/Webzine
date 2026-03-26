using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;
using Webzine.Entity.Fixtures;

namespace Webzine.EntitiesContext.Seeders
{
    public class SeedDataLocal
    {
        
        public static WebzineData Initialize()
        {
            var data = new WebzineData();
            var styles = StyleFactory.CreerStyles(18);
            var artistes = ArtisteFactory.CreerArtistes(8);
            var titres = TitreFactory.CreerTitres(styles, artistes, 70);
            var commentaires = CommentaireFactory.CreerCommentaires(titres, 100);

            data.styles = styles;
            data.artistes = artistes;
            data.titres = titres;
            data.commentaires = commentaires;

            return data;
        }
    }
}
