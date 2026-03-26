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
            var styles = LocalStylesFactory.CreerStyles();
            var artistes = ArtisteFactory.CreerArtistes();
            var titres = TitreFactory.CreerTitres(styles, artistes, 50);
            var commentaires = CommentaireFactory.CreerCommentaires();

            data.styles = styles;
            data.artistes = artistes;
            data.titres = titres;
            data.commentaires = commentaires;

            return data;
        }
    }
}
