namespace Webzine.Business.Contracts.Dto
{

    /// <summary>
    /// Représente un objet de transfert de données contenant les statistiques principales et les informations
    /// synthétiques pour l'affichage du dashboard de l'application.
    /// </summary>
    /// <remarks>Utilisez cette classe pour transmettre un ensemble de données agrégées concernant les
    /// artistes, titres, styles, lectures, likes et autres indicateurs clés à des fins d'affichage ou de reporting dans
    /// une interface utilisateur de type tableau de bord.</remarks>
    public class DashboardDto
    {
        /// <summary>
        /// Obtient ou définit le nombre total d'artistes.
        /// </summary>
        public int NbArtistes { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre total de titres associés à l'entité.
        /// </summary>
        public int NbTitres { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre total de style de lusique sur l'application.
        /// </summary>
        public int NbStyles { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre total de lectures de musiques sur l'application.
        /// </summary>
        public int NbLectures { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre de likes total.
        /// </summary>
        public int NbLikes { get; set; }

        /// <summary>
        /// Obtient ou définit le nombre de biographies d'artistes disponibles.
        /// </summary>
        public int NbBiographiesArtistes { get; set; }

        /// <summary>
        /// Obtient ou définit le titre de l'article le plus lu.
        /// </summary>
        public string TitrePlusLu { get; set; }

        /// <summary>
        /// Obtient ou définit  l'artiste les plus chroniqué.
        /// </summary>
        public string ArtistesPlusChroniques { get; set; }

        /// <summary>
        /// Obtient ou définit l'artiste avec le plus de titres provenant d'albums distincts.
        /// </summary>
        public string ArtistePlusAlbums { get; set; }
    }
}
