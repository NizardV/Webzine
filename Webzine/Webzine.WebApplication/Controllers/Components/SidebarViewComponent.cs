using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;

namespace Webzine.WebApplication.Controllers.Components
{
    public class SidebarViewComponent : ViewComponent
    {

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var styles = LocalStylesFactory.CreerStyles();

            return View(styles);
        }
    }
}
