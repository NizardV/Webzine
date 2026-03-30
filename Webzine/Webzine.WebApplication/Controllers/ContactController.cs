using Microsoft.AspNetCore.Mvc;

namespace Webzine.WebApplication.Controllers
{
    public class ContactController : Controller
    {
        /// <summary>
        /// Retourne la page contact.
        /// </summary>
        /// <returns>Une vue.</returns>
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
