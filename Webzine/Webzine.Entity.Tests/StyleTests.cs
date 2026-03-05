namespace Webzine.Entity.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests de l'entité <see cref="Style"/>.
    /// Vérifie que les contraintes imposées (nom du champ, longueur max du champ,
    /// champ obligatoire, libellé du champ, clé primaire...) sont bien respectées.
    /// </summary>
    [TestClass]
    public class StyleTests
    {
        [TestMethod]
        public void StyleHasIdStyle()
        {
            Common.HasProperty(typeof(Style), nameof(Style.IdStyle));
        }

        [TestMethod]
        public void StyleHasLibelle()
        {
            Common.HasProperty(typeof(Style), nameof(Style.Libelle));
        }

        [TestMethod]
        public void StyleHasLibelleDisplayValid()
        {
            Common.AttributDisplay(typeof(Style), nameof(Style.Libelle), "Libellé");
        }

        [TestMethod]
        public void StyleHasLibelleRequis()
        {
            Common.AttributRequis(typeof(Style), nameof(Style.Libelle));
        }

        [TestMethod]
        public void StyleHasLibelleTailleMin2()
        {
            Common.AttributLongueurMin(typeof(Style), nameof(Style.Libelle), 2);
        }

        [TestMethod]
        public void StyleHasLibelleTailleMax50()
        {
            Common.AttributLongueurMax(typeof(Style), nameof(Style.Libelle), 50);
        }
    }
}
