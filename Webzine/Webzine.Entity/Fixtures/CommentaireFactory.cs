using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;

namespace Webzine.Entity.Fixtures
{
    public class CommentaireFactory
    {
        public static List<Commentaire> GetCommentaires()
        {
            var commentaires = new List<Commentaire>();
            commentaires.Add(new Commentaire
            {
                IdCommentaire = 1,
                Contenu = "C'est un super article !",
                Auteur = "Jean Dupont",
                DateCreation = DateOnly.FromDateTime(DateTime.Now),
                IdTitre = 1,
                Titre = new Titre
                {
                    IdTitre = 1,
                    Libelle = "Chanson 1"
                }
            });
            commentaires.Add(new Commentaire
            {
                IdCommentaire = 2,
                Contenu = "Super chanson !",
                Auteur = "Michel",
                DateCreation = DateOnly.FromDateTime(DateTime.Now),
                IdTitre = 2,
                Titre = new Titre
                {
                    IdTitre = 2,
                    Libelle = "Chanson 2"
                }
            });
            commentaires.Add(new Commentaire
            {
                IdCommentaire = 3,
                Contenu = "Cette chanson me rappelle mon enfance",
                Auteur = "xxxGamerxxx",
                DateCreation = DateOnly.FromDateTime(DateTime.Now),
                IdTitre = 3,
                Titre = new Titre
                {
                    IdTitre = 3,
                    Libelle = "Chanson 3"
                }
            });
            commentaires.Add(new Commentaire
            {
                IdCommentaire = 4,
                Contenu = "Cette chanson est géniale",
                Auteur = "BonjourCMoi",
                DateCreation = DateOnly.FromDateTime(DateTime.Now),
                IdTitre = 3,
                Titre = new Titre
                {
                    IdTitre = 3,
                    Libelle = "Chanson 3"
                }
            });

            return commentaires;

        }
        public List<Commentaire> GetCommentairesByTitre(int idTitre)
        {
            var commentaires = GetCommentaires();
            return commentaires.Where(c => c.IdTitre == idTitre).ToList();
        }
    }
}
