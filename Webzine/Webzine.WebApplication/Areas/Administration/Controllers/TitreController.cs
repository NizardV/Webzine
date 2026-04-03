namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Webzine.Entity;
    using Webzine.Business.Contracts.Services;
    using Webzine.Repository.Contracts;

    [Area("administration")]
    /// <summary>
    /// Contrôleur d'administration des titres.
    /// Cette partie est orientée IHM : aucune persistance n'est effectuée.
    /// Les données proviennent des Factory et les modifications/créations sont simulées via TempData.
    /// </summary>
    public class TitreController : Controller
    {
        private readonly ITitreRepository _titreRepository;
        
        /// <summary>
        /// Initializes a new instance of the TitreController class with the specified logger and titre repository.
        /// </summary>
        /// <param name="logger">The logger instance used to log information, warnings, and errors for this controller. Cannot be null.</param>
        /// <param name="titreRepository">The repository used to access and manage titre data. Cannot be null.</param>
        public TitreController(ILogger<TitreController> logger, ITitreRepository titreRepository)
        {
            this._titreRepository = titreRepository;
        }

        /// <summary>
        /// Retourne la vue qui affiche la liste de tous les titres disponibles.
        /// </summary>
        /// <returns>Une vue.</returns>
        public IActionResult Index()
        {
            var titres = this._titreRepository.FindAll();
            return this.View(titres);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        /// <summary>
        /// Créer un nouveau titre.
        /// </summary>
        /// <param name="titre">L'entité titre a créer.</param>
        /// <returns>Une redirection sur la page index.</returns>
        [HttpPost]
        public IActionResult Create(Titre titre)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(titre);
            }

            this._titreRepository.Add(titre);

            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// Retourne la vue pour modifier un titre.
        /// </summary>
        /// <param name="id">L'identifiant du titre à modifier.</param>
        /// <returns>Une vue.</returns>
        public IActionResult Edit(int id)
        {
            var titre = this._titreRepository.Find(id);
            if (titre == null)
            {
                return this.NotFound();
            }

            return this.View(titre);
        }

        /// <summary>
        /// Modifier un titre existant.
        /// </summary>
        /// <param name="id">L'identifiant du titre à modifier</param>
        /// <param name="titre">L'objet titre à modifier</param>
        /// <returns>Une redirection sur la page index.</returns>
        [HttpPost]
        public IActionResult Edit(int id, Titre titre)
        {
            if (id != titre.IdTitre)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(titre);
            }

            this._titreRepository.Update(titre);

            return this.RedirectToAction(nameof(this.Index));
        }

        /// <summary>
        /// Retourne la vue pour supprimer un titre.
        /// </summary>
        /// <param name="id">L'identifiant du titre à modifier.</param>
        /// <returns>Une vue.</returns>
        public IActionResult Delete(int id)
        {
            var titre = this._titreRepository.Find(id);
            if (titre == null)
            {
                return this.NotFound();
            }

            return this.View(titre);
        }

        /// <summary>
        /// Supprimer un titre.
        /// </summary>
        /// <param name="id">L'identifiant du titre à supprimer</param>
        /// <returns>Une redirection sur la page index.</returns>
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var titre = this._titreRepository.Find(id);
            if (titre == null)
            {
                return this.NotFound();
            }

            this._titreRepository.Delete(titre);

            return this.RedirectToAction(nameof(this.Index));
        }
    }
}
