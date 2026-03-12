using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.WebApplication.ViewModels;

namespace Webzine.WebApplication.Controllers
{
    public class TitreController : Controller
    {
        [HttpGet]
        [Route("titre/{id}")]
        public IActionResult DetailsTitre(int id)
        {
            var titre = TitreFactory.GetTitreById(id);
            if (titre is null)
            {
                return NotFound();
            }
            var commentaires = CommentaireFactory.GetCommentairesByTitre(id);
            return View(new TitreModel { Titre = titre, Commentaires = commentaires});
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
