namespace Webzine.WebApplication.ViewModels
{
    /// <summary>
    /// Modèle de vue utilisé pour la page d'accueil.
    /// Contient la liste paginée des derniers titres chroniqués ainsi que les informations de pagination,
    /// et la liste complète des titres (utilisée pour les blocs annexes comme "Titres les plus populaires").
    /// </summary>
    public class HomeViewModel
    {
        /// <summary>
        /// Obtient la liste complète des titres.
        /// </summary>
        public required IReadOnlyList<TitreModel> AllTitres { get; init; }

        /// <summary>
        /// Obtient la liste paginée des titres à afficher sur la page courante.
        /// </summary>
        public required IReadOnlyList<TitreModel> PagedTitres { get; init; }

        /// <summary>
        /// Obtient le nombre d'éléments par page.
        /// </summary>
        public required int PageSize { get; init; }

        /// <summary>
        /// Obtient l'index de la page courante (0-based).
        /// </summary>
        public required int CurrentPage { get; init; }

        /// <summary>
        /// Obtient l'index maximal de page (0-based).
        /// </summary>
        public required int MaxPage { get; init; }
    }
}
