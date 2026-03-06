using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.WebApplication.ViewModels;

namespace Webzine.WebApplication.Controllers
{
    public class TitreController : Controller
    {
        [HttpGet]
        [Route("Titre/DetailsTitre/{id}")]
        public IActionResult DetailsTitre(int id)
        {
            var titre = TitreFactory.GetTitreById(id);
            if (titre is null)
            {
                return NotFound();
            }
            return View(new TitreModel { Titre = titre});
        }
    }
}
