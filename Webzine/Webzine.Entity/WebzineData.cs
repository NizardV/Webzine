using System;
using System.Collections.Generic;
using System.Text;

namespace Webzine.Entity
{
    public class WebzineData
    {
        public ICollection<Style> styles { get; set; } = new List<Style>();
        public ICollection<Commentaire> commentaires { get; set; } = new List<Commentaire>();
        public ICollection<Artiste> artistes { get; set; } = new List<Artiste>();
        public ICollection<Titre> titres { get; set; } = new List<Titre>();
    }
}
