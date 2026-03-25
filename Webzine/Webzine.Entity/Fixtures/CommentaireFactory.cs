using Bogus;
using System;
using System.Collections.Generic;
using System.Text;
using Webzine.Entity;

namespace Webzine.Entity.Fixtures
{
    public class CommentaireFactory
    {
        // On crée une variable statique pour stocker la liste. 
        // Ainsi, les commentaires ne changent pas à chaque fois qu'on rafraîchit la page !
        private static List<Commentaire>? _commentaires;
        public static List<Commentaire> CreerCommentaires()
        {
            // Si la liste a déjà été générée, on la retourne directement
            if (_commentaires != null)
            {
                return _commentaires;
            }

            // Ça permet d'avoir toujours les mêmes fausses données à chaque lancement de l'app.
            Randomizer.Seed = new Random(12345);

            // On configure les règles de génération pour l'entité Commentaire
            var commentaireFaker = new Faker<Commentaire>("fr") // "fr" pour générer du texte en français
                .RuleFor(c => c.IdCommentaire, f => f.IndexGlobal + 1) // Auto-incrément : 1, 2, 3...
                .RuleFor(c => c.Auteur, f => f.Internet.UserName()) // Génère un pseudo réaliste
                .RuleFor(c => c.DateCreation, f => DateOnly.FromDateTime(f.Date.Recent(60))) // Une date dans les 60 derniers jours
                .RuleFor(c => c.Contenu, f => f.Lorem.Paragraph()) // Génère un paragraphe de "Lorem Ipsum"
                .RuleFor(c => c.IdTitre, f => f.Random.Number(1, 10)); // On génère juste un faux ID entre 1 et 10 (selon le nombre de titres prévu)

            // On demande à Bogus de générer 30 commentaires d'un coup
            _commentaires = commentaireFaker.Generate(30);

            return _commentaires;
        }
        public static List<Commentaire> GetCommentairesByTitre(int idTitre)
        {
            var commentaires = CreerCommentaires();
            return commentaires.Where(c => c.IdTitre == idTitre).ToList();
        }
    }
}
