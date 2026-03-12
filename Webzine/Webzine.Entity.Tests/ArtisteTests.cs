namespace Webzine.Entity.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Webzine.Entity;

    /// <summary>
    /// Tests de l'entité <see cref="Artiste"/>.
    /// Vérifie que les contraintes imposées (nom du champ, longueur max du champ,
    /// champ obligatoire, libellé du champ, clé primaire...) sont bien respectées.
    /// </summary>
    [TestClass]
    public class ArtisteTests
    {
        [TestMethod]
        public void ArtisteHasIdArtiste()
        {
            Common.HasProperty(typeof(Artiste), nameof(Artiste.IdArtiste));
        }

        [TestMethod]
        public void ArtisteHasNom()
        {
            Common.HasProperty(typeof(Artiste), nameof(Artiste.Nom));
        }

        [TestMethod]
        public void ArtisteHasNomTailleMin1()
        {
            Common.AttributLongueurMin(typeof(Artiste), nameof(Artiste.Nom), 2);
        }

        [TestMethod]
        public void ArtisteHasNomTailleMax50()
        {
            Common.AttributLongueurMax(typeof(Artiste), nameof(Artiste.Nom), 50);
        }

        [TestMethod]
        public void ArtisteHasNomDisplayValid()
        {
            Common.AttributDisplay(typeof(Artiste), nameof(Artiste.Nom), "Nom de l'artiste");
        }

        [TestMethod]
        public void ArtisteHasBiographie()
        {
            Common.HasProperty(typeof(Artiste), nameof(Artiste.Biographie));
        }

        [TestMethod]
        public void ArtisteHasTitres()
        {
            Common.HasProperty(typeof(Artiste), nameof(Artiste.Titres));
        }
    }
}
