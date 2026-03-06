namespace Webzine.Entity.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests de l'entité <see cref="Titre"/>.
    /// Vérifie que les contraintes imposées (nom du champ, longueur max du champ,
    /// champ obligatoire, libellé du champ, clé primaire...) sont bien respectées.
    /// </summary>
    [TestClass]
    public class TitreTests
    {
        [TestMethod]
        public void TitreHasIdTitre()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.IdTitre));
        }

        [TestMethod]
        public void TitreHasIdArtiste()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.IdArtiste));
        }

        [TestMethod]
        public void TitreHasArtiste()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.Artiste));
        }

        [TestMethod]
        public void TitreHasLibelle()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.Libelle));
        }

        [TestMethod]
        public void TitreHasLibelleDisplayValid()
        {
            Common.AttributDisplay(typeof(Titre), nameof(Titre.Libelle), "Titre");
        }

        [TestMethod]
        public void TitreHasLibelleRequis()
        {
            Common.AttributRequis(typeof(Titre), nameof(Titre.Libelle));
        }

        [TestMethod]
        public void TitreHasLibelleTailleMin1()
        {
            Common.AttributLongueurMin(typeof(Titre), nameof(Titre.Libelle), 1);
        }

        [TestMethod]
        public void TitreHasLibelleTailleMax200()
        {
            Common.AttributLongueurMax(typeof(Titre), nameof(Titre.Libelle), 200);
        }

        [TestMethod]
        public void TitreHasChronique()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.Chronique));
        }

        [TestMethod]
        public void TitreHasChroniqueRequis()
        {
            Common.AttributRequis(typeof(Titre), nameof(Titre.Chronique));
        }

        [TestMethod]
        public void TitreHasChroniqueTailleMin10()
        {
            Common.AttributLongueurMin(typeof(Titre), nameof(Titre.Chronique), 10);
        }

        [TestMethod]
        public void TitreHasChroniqueTailleMax4000()
        {
            Common.AttributLongueurMax(typeof(Titre), nameof(Titre.Chronique), 4000);
        }

        [TestMethod]
        public void TitreHasDateCreation()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.DateCreation));
        }

        [TestMethod]
        public void TitreHasDateCreationRequis()
        {
            Common.AttributRequis(typeof(Titre), nameof(Titre.DateCreation));
        }

        [TestMethod]
        public void TitreHasDateCreationDisplayValid()
        {
            Common.AttributDisplay(typeof(Titre), nameof(Titre.DateCreation), "Date de création");
        }

        [TestMethod]
        public void TitreHasDuree()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.Duree));
        }

        [TestMethod]
        public void TitreHasDureeDisplayValid()
        {
            Common.AttributDisplay(typeof(Titre), nameof(Titre.Duree), "Durée en secondes");
        }

        [TestMethod]
        public void TitreHasDateSortie()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.DateSortie));
        }

        [TestMethod]
        public void TitreHasDateSortieRequis()
        {
            Common.AttributRequis(typeof(Titre), nameof(Titre.DateSortie));
        }

        [TestMethod]
        public void TitreHasDateSortieDisplayValid()
        {
            Common.AttributDisplay(typeof(Titre), nameof(Titre.DateSortie), "Date de sortie");
        }

        [TestMethod]
        public void TitreHasUrlJaquette()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.UrlJaquette));
        }

        [TestMethod]
        public void TitreHasUrlJaquetteRequis()
        {
            Common.AttributRequis(typeof(Titre), nameof(Titre.UrlJaquette));
        }

        [TestMethod]
        public void TitreHasUrlJaquetteDisplayValid()
        {
            Common.AttributDisplay(typeof(Titre), nameof(Titre.UrlJaquette), "Jaquette de l'album");
        }

        [TestMethod]
        public void TitreHasUrlJaquetteTailleMax250()
        {
            Common.AttributLongueurMax(typeof(Titre), nameof(Titre.UrlJaquette), 250);
        }

        [TestMethod]
        public void TitreHasUrlEcoute()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.UrlEcoute));
        }

        [TestMethod]
        public void TitreHasUrlEcouteDisplayValid()
        {
            Common.AttributDisplay(typeof(Titre), nameof(Titre.UrlEcoute), "URL d'écoute");
        }

        [TestMethod]
        public void TitreHasUrlEcouteTailleMin13()
        {
            Common.AttributLongueurMin(typeof(Titre), nameof(Titre.UrlEcoute), 13);
        }

        [TestMethod]
        public void TitreHasUrlEcouteTailleMax250()
        {
            Common.AttributLongueurMax(typeof(Titre), nameof(Titre.UrlEcoute), 250);
        }

        [TestMethod]
        public void TitreHasNbLectures()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.NbLectures));
        }

        [TestMethod]
        public void TitreHasNbLecturesDisplayValid()
        {
            Common.AttributDisplay(typeof(Titre), nameof(Titre.NbLectures), "Nombre de lectures");
        }

        [TestMethod]
        public void TitreHasNbLecturesRequis()
        {
            Common.AttributRequis(typeof(Titre), nameof(Titre.NbLectures));
        }

        [TestMethod]
        public void TitreHasNbLikes()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.NbLikes));
        }

        [TestMethod]
        public void TitreHasNbLikesDisplayValid()
        {
            Common.AttributDisplay(typeof(Titre), nameof(Titre.NbLikes), "Nombre de likes");
        }

        [TestMethod]
        public void TitreHasNbLikesRequis()
        {
            Common.AttributRequis(typeof(Titre), nameof(Titre.NbLikes));
        }

        [TestMethod]
        public void TitreHasAlbum()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.Album));
        }

        [TestMethod]
        public void TitreHasAlbumRequis()
        {
            Common.AttributRequis(typeof(Titre), nameof(Titre.Album));
        }

        [TestMethod]
        public void TitreHasCommentaires()
        {
            Common.HasProperty(typeof(Titre), nameof(Titre.Commentaires));
        }

        [TestMethod]
        public void TitreUrlJaquetteeIsNotMandatory()
        {
            Common.AttributHasNotUrlValidation(typeof(Titre), nameof(Titre.UrlJaquette));
        }
    }
}
