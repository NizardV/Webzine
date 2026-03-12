namespace Webzine.WebApplication.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Webzine.Entity;
    using Webzine.Entity.Fixtures;
    using Webzine.WebApplication.ViewModels;

    /// <summary>
    /// Ce contrôleur gère les requêtes liées à la page d'accueil de l'application Webzine.
    /// </summary>
    public class HomeController : Controller
    {
        /// <summary>
        /// Retourne la vue principale de l'application.
        /// </summary>
        /// <returns>Un objet IActionResult qui rend la vue associée à la page d'accueil.</returns>
        [HttpGet]
        public IActionResult Home(int page = 0)
        {
            const int pageSize = 3;

            var titres = TitreFactory.GetTitres().Select(t => new TitreModel { Titre = t }).ToList();
            var totalItems = titres.Count;
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

        /// <summary>
        /// Retourne la vue de détail d'un titre spécifique.
        /// </summary>
        /// <param name="id">Id du Titre</param>
        /// <returns>Une vue</returns>
        public IActionResult DetailTitre(int id)
        {
            Titre? titre = TitreFactory.GetTitreById(id);

            if (titre == null)
            {
                return this.NotFound();
            }

            var commentaires = CommentaireFactory.GetCommentairesByTitre(id);

            TitreModel model = new TitreModel
            {
                Titre = titre,
                Commentaires = commentaires,
            };

            return this.View("/Views/Titre/DetailsTitre.cshtml", model);
        }
    }
}
