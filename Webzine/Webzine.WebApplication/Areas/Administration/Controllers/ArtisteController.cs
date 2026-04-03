using Microsoft.AspNetCore.Mvc;
using Webzine.Entity;
using Webzine.Repository.Contracts;
using Webzine.WebApplication.Areas.Administration.ViewModel.Artiste;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    /// <summary>
    /// Contrôleur ASP.NET Core MVC pour la gestion des entités Artiste dans la zone d'administration. Fournit des
    /// actions pour afficher, créer, modifier et supprimer des artistes via l'interface d'administration.
    /// </summary>
    /// <remarks>Ce contrôleur utilise un référentiel d'artistes pour accéder et manipuler les données des
    /// artistes. Toutes les actions sont restreintes à la zone "administration". Les méthodes prennent en charge la
    /// validation du modèle et gèrent les cas d'entités non trouvées ou d'identifiants non valides. Ce contrôleur est
    /// destiné à être utilisé dans un contexte d'administration sécurisé.</remarks>
    [Area("administration")]
    public class ArtisteController : Controller
    {
        private readonly IArtisteRepository artisteRepository;

        /// <summary>
        /// Initialise une nouvelle instance de la classe ArtisteController avec le référentiel d'artistes spécifié.
        /// </summary>
        /// <param name="artisteRepository">Le référentiel utilisé pour accéder et gérer les données des artistes. Ne peut pas être null.</param>
        public ArtisteController(IArtisteRepository artisteRepository)
        {
            this.artisteRepository = artisteRepository;
        }

        /// <summary>
        /// Handles HTTP GET requests for the default page and displays a list of all artistes.
        /// </summary>
        /// <returns>A view that renders the list of artistes retrieved from the repository.</returns>
        public IActionResult Index(int page = 0)
        {
            int limite = 50;
            var totalArtistes = this.artisteRepository.Count();
            var maxPage = (int)Math.Ceiling(totalArtistes / (double)limite) - 1;

            if (page < 0) page = 0;
            if (page > maxPage && maxPage >= 0) page = maxPage;

            var artistes = this.artisteRepository.FindArtistes(page * limite, limite);
            var viewModel = new AdminArtisteIndexViewModel
            {
                PagedArtistes = artistes,
                CurrentPage = page,
                MaxPage = maxPage
            };
            return this.View(viewModel);
        }

        /// <summary>
        /// Returns a view for creating a new artiste.
        /// </summary>
        /// <remarks>The returned view is initialized with a new instance of the Artiste model. This
        /// method is typically used to render the creation form in response to a GET request.</remarks>
        /// <returns>A view that displays a form for entering details of a new artiste.</returns>
        public IActionResult Create()
        {
            return this.View(new Artiste());
        }

        
        [HttpPost]
        public IActionResult Create(Artiste artiste)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(artiste);
            }

            this.artisteRepository.Add(artiste);
            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Edit(int id)
        {
            var artiste = this.artisteRepository.Find(id);

            if (artiste == null)
            {
                return this.NotFound();
            }

            return this.View(artiste);
        }

        [HttpPost]
        public IActionResult Edit(int id, Artiste artiste)
        {
            if (id != artiste.IdArtiste)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(artiste);
            }

            this.artisteRepository.Update(artiste);
            return this.RedirectToAction(nameof(this.Index));
        }

        public IActionResult Delete(int id)
        {
            var artiste = this.artisteRepository.Find(id);

            if (artiste == null)
            {
                return this.NotFound();
            }

            return this.View(artiste);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var artiste = this.artisteRepository.Find(id);
            if (artiste != null)
            {
                this.artisteRepository.Delete(artiste);
            }

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
