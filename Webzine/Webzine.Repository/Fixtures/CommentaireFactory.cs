using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;

namespace Webzine.Repository.Factories
{
    public class CommentaireFactory
    {
        public List<Commentaire> GetCommentaires()
        {
            return [
                new Commentaire{
                    Id = 1,
                    Libelle = "C'est un super article !",
                    Pseudo = "Jean Dupont",
                    DateCreation = DateOnly.FromDateTime(DateTime.Now),
                    TitreId = 1,
                },
                new Commentaire{
                    Id = 2,
                    Libelle = "Super chanson !",
                    Pseudo = "Michel",
                    DateCreation = DateOnly.FromDateTime(DateTime.Now),
                    TitreId = 2,
                },
                new Commentaire{
                    Id = 3,
                    Libelle = "Cette chanson me rappelle mon enfance",
                    Pseudo = "xxxGamerxxx",
                    DateCreation = DateOnly.FromDateTime(DateTime.Now),
                    TitreId = 3,
                }
                ];
        }
    }
}
