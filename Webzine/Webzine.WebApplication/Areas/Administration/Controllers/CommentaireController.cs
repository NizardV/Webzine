using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.WebApplication.Areas.Administration.ViewModel.Commentaire;

namespace Webzine.WebApplication.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Route("administration")]
    public class CommentaireController : Controller
    {
        [Route("Commentaires")]
        public IActionResult Commentaires()
        {
            var commentaires = CommentaireFactory.CreerCommentaires();
            return View(new CommentairesAdministrationModel {Commentaires = commentaires});
        }

        [HttpGet]
        [Route("Commentaires/Supprimer/{id}")]
        public IActionResult Supprimer(int id)
        {
            var commentaire = CommentaireFactory.CreerCommentaires().FirstOrDefault(c => c.IdCommentaire == id);
            if (commentaire is not null)
            {
                return View(commentaire);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
