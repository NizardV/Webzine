using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;

namespace Webzine.Business.Contracts.DTO
{
    public class PlaylistExtractionResult
    {
        public List<Artiste> Artistes { get; set; } = new List<Artiste>();
        public List<Titre> Titres { get; set; }= new List<Titre>();
    }
}
