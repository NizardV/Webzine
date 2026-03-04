using Microsoft.AspNetCore.Mvc;

namespace Webzine.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
