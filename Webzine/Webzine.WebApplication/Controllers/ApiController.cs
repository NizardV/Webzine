using Microsoft.AspNetCore.Mvc;
using Webzine.WebApplication.ViewModels;
namespace Webzine.WebApplication.Controllers
{

    public class ApiController : Controller
    {
        private readonly IConfiguration _configuration;

        public ApiController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Version()
        {
            return Ok(new ApiModel { nom = "webzine", version = _configuration["version"] });
        }
    }
}
