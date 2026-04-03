namespace Webzine.WebApplication.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Webzine.Entity;
    using Webzine.Entity.Fixtures;
    using Webzine.Repository.Contracts;
    using Webzine.WebApplication.ViewModels;

    /// <summary>
    /// Ce contrôleur gère les requêtes liées à la page d'accueil de l'application Webzine.
    /// </summary>
    public class HomeController : Controller
    {
        private ITitreRepository _titreRepository;

        public HomeController(ITitreRepository titreRepository)
        {
            this._titreRepository = titreRepository;
        }

        /// <summary>{
        /// Retourne la vue principale de l'application.
        /// </summary>
        /// <returns>Un objet IActionResult qui rend la vue associée à la page d'accueil.</returns>
        public IActionResult Index(int page = 0)
        {
            const int pageSize = 3;

            var titres = this._titreRepository.FindAll();
            var totalItems = titres.Count();
            var maxPage = totalItems == 0 ? 0 : (int)Math.Ceiling(totalItems / (double)pageSize) - 1;
            var currentPage = Math.Clamp(page, 0, maxPage);

            var vm = new HomeViewModel
            {
                AllTitres = titres,
                PageSize = pageSize,
                CurrentPage = currentPage,
                MaxPage = maxPage,
                PagedTitres = titres.Skip(currentPage * pageSize).Take(pageSize).ToList(),
            };

            return this.View(vm);
        }
    }
}
