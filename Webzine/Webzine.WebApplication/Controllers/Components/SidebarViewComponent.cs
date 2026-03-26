using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.Repository.Contracts;

namespace Webzine.WebApplication.Controllers.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        public IStyleRepository _styleRepository;
        public SidebarViewComponent(IStyleRepository styleRepository) {
            _styleRepository = styleRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var styles = _styleRepository.FindAll();
            return View(styles);
        }
    }
}
