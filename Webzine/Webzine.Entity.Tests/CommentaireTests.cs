namespace Webzine.Entity.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests de l'entité <see cref="Commentaire"/>.
    /// Vérifie que les contraintes imposées (nom du champ, longueur max du champ,
    /// champ obligatoire, libellé du champ, clé primaire...) sont bien respectées.
    /// </summary>
    [TestClass]
    public class CommentaireTests
    {
        [TestMethod]
        public void CommentaireHasIdCommentaire()
        {
            Common.HasProperty(typeof(Commentaire), nameof(Commentaire.IdCommentaire));
        }

        [TestMethod]
        public void CommentaireHasContenu()
        {
            Common.HasProperty(typeof(Commentaire), nameof(Commentaire.Contenu));
        }

        [TestMethod]
        public void CommentaireHasContenuDisplayValid()
        {
            Common.AttributDisplay(typeof(Commentaire), nameof(Commentaire.Contenu), "Commentaire");
        }

        [TestMethod]
        public void CommentaireHasContenuRequis()
        {
            Common.AttributRequis(typeof(Commentaire), nameof(Commentaire.Contenu));
        }

        [TestMethod]
        public void CommentaireHasContenuTailleMin10()
        {
            Common.AttributLongueurMin(typeof(Commentaire), nameof(Commentaire.Contenu), 10);
        }

        [TestMethod]
        public void CommentaireHasContenuTailleMax1000()
        {
            Common.AttributLongueurMax(typeof(Commentaire), nameof(Commentaire.Contenu), 1000);
        }

        [TestMethod]
        public void CommentaireHasAuteur()
        {
            Common.HasProperty(typeof(Commentaire), nameof(Commentaire.Auteur));
        }

        [TestMethod]
        public void CommentaireHasAuteurDisplayValid()
        {
            Common.AttributDisplay(typeof(Commentaire), nameof(Commentaire.Auteur), "Nom");
        }

        [TestMethod]
        public void CommentaireHasAuteurRequis()
        {
            Common.AttributRequis(typeof(Commentaire), nameof(Commentaire.Auteur));
        }

        [TestMethod]
        public void CommentaireHasAuteurTailleMin2()
        {
            Common.AttributLongueurMin(typeof(Commentaire), nameof(Commentaire.Auteur), 2);
        }

        [TestMethod]
        public void CommentaireHasAuteurTailleMax30()
        {
            Common.AttributLongueurMax(typeof(Commentaire), nameof(Commentaire.Auteur), 30);
        }

        [TestMethod]
        public void CommentaireHasDateCreation()
        {
            Common.HasProperty(typeof(Commentaire), nameof(Commentaire.DateCreation));
        }

        [TestMethod]
        public void CommentaireHasDateCreationRequis()
        {
            Common.AttributRequis(typeof(Commentaire), nameof(Commentaire.DateCreation));
        }

        [TestMethod]
        public void CommentaireHasDateCreationDisplayValid()
        {
            Common.AttributDisplay(typeof(Commentaire), nameof(Commentaire.DateCreation), "Date de création");
        }

        [TestMethod]
        public void CommentaireHasIdTitre()
        {
            Common.HasProperty(typeof(Commentaire), nameof(Commentaire.IdTitre));
        }

        [TestMethod]
        public void CommentaireHasTitre()
        {
            Common.HasProperty(typeof(Commentaire), nameof(Commentaire.Titre));
        }
    }
}
