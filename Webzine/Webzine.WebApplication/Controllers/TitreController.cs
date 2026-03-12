using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.WebApplication.ViewModels;

namespace Webzine.WebApplication.Controllers
{
    /// <summary>
    /// Contrôleur gérant l'affichage des titres côté site public (détails, filtrage par style)
    /// ainsi que quelques interactions IHM sans persistance (like, édition légère).
    /// Les données restent issues des Factory.
    /// </summary>
    public class TitreController : Controller
    {
        private static string GetLikesKey(int id) => $"ui:titre:{id}:likes";

        private static string GetChroniqueKey(int id) => $"ui:titre:{id}:chronique";

        [HttpGet]
        [Route("Titre/DetailsTitre/{id}")]
        /// <summary>
        /// Affiche la page de détail d'un titre.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        public IActionResult DetailsTitre(int id)
        {
            var titre = TitreFactory.GetTitreById(id);
            if (titre is null)
            {
                return NotFound();
            }

            if (TempData.Peek(GetLikesKey(id)) is object likesObj
                && int.TryParse(likesObj?.ToString(), out var likes))
            {
                titre.NbLikes = likes;
            }

            if (TempData.Peek(GetChroniqueKey(id)) is object chroniqueObj)
            {
                titre.Chronique = chroniqueObj?.ToString() ?? titre.Chronique;
            }
            var commentaires = CommentaireFactory.GetCommentairesByTitre(id);
            return View(new TitreModel { Titre = titre, Commentaires = commentaires});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Titre/Like/{id}")]
        /// <summary>
        /// Incrémente le compteur de likes côté IHM.
        /// L'opération n'est pas persistée (Factory inchangées) et sert uniquement à refléter l'état dans l'interface.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        public IActionResult Like(int id)
        {
            var titre = TitreFactory.GetTitreById(id);
            if (titre is null)
            {
                return NotFound();
            }

            var current = titre.NbLikes;
            if (TempData.Peek(GetLikesKey(id)) is object likesObj
                && int.TryParse(likesObj?.ToString(), out var likes))
            {
                current = likes;
            }

            TempData[GetLikesKey(id)] = current + 1;
            return RedirectToAction(nameof(DetailsTitre), new { id });
        }

        [HttpGet]
        [Route("Titre/Edit/{id}")]
        /// <summary>
        /// Affiche un formulaire d'édition légère (IHM) pour un titre.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        public IActionResult Edit(int id)
        {
            var titre = TitreFactory.GetTitreById(id);
            if (titre is null)
            {
                return NotFound();
            }

            if (TempData.Peek(GetChroniqueKey(id)) is object chroniqueObj)
            {
                titre.Chronique = chroniqueObj?.ToString() ?? titre.Chronique;
            }

            return View(titre);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Titre/Edit/{id}")]
        /// <summary>
        /// Enregistre l'édition légère (IHM) d'un titre, sans persistance.
        /// </summary>
        /// <param name="id">Identifiant du titre.</param>
        /// <param name="chronique">Nouvelle chronique à afficher.</param>
        public IActionResult Edit(int id, [FromForm] string chronique)
        {
            var titre = TitreFactory.GetTitreById(id);
            if (titre is null)
            {
                return NotFound();
            }

            if (!string.IsNullOrWhiteSpace(chronique))
            {
                TempData[GetChroniqueKey(id)] = chronique;
            }

            return RedirectToAction(nameof(DetailsTitre), new { id });
        }

        [HttpGet]
        [Route("titres/style/{libelle}")]
        public IActionResult Style(string libelle)
        {
            // On utilise la méthode pour trouver le style correspondant au libellé de l'URL
            var style = LocalStylesFactory.GetStyleByLibelle(libelle);

            if (style == null)
            {
                return NotFound();
            }

            // On récupère tous les titres de la Factory
            // puis on filtre (.Where) pour ne garder que ceux qui contiennent ce style précis.
            var titresDuStyle = TitreFactory.GetTitres()
                .Where(t => t.Styles.Any(s => s.IdStyle == style.IdStyle))
                .ToList();

            // On stocke le vrai nom du style (avec la bonne majuscule) pour l'afficher dans le <h1> de la vue
            ViewBag.NomDuStyle = style.Libelle;

            // On envoie la liste filtrée à la vue
            return View(titresDuStyle);
        }
    }
}
