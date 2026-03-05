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
                    IdCommentaire = 1,
                    Contenu = "C'est un super article !",
                    Auteur = "Jean Dupont",
                    DateCreation = DateOnly.FromDateTime(DateTime.Now),
                    IdTitre = 1,
                },
                new Commentaire{
                    IdCommentaire = 2,
                    Contenu = "Super chanson !",
                    Auteur = "Michel",
                    DateCreation = DateOnly.FromDateTime(DateTime.Now),
                    IdTitre = 2,
                },
                new Commentaire{
                    IdCommentaire = 3,
                    Contenu = "Cette chanson me rappelle mon enfance",
                    Auteur = "xxxGamerxxx",
                    DateCreation = DateOnly.FromDateTime(DateTime.Now),
                    IdTitre = 3,
                }
                ];
        }
    }
}
