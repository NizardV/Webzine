using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.Repository.Contracts;

namespace Webzine.WebApplication.Controllers.Components
{
    public class SidebarViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var styles = _styleRepository.FindAll();
            return View(styles);
        }
    }
}
