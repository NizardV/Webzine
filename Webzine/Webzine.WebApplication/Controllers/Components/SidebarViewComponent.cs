using Microsoft.AspNetCore.Mvc;
using Webzine.Entity;
using Webzine.Entity.Fixtures;
using Webzine.Repository.Contracts;
using Webzine.WebApplication.Areas.Administration.Controllers;

namespace Webzine.WebApplication.Controllers.Components
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly ILogger<SidebarViewComponent> _logger;

        public IStyleRepository _styleRepository;
        public SidebarViewComponent(IStyleRepository styleRepository, ILogger<SidebarViewComponent> logger) {
            _styleRepository = styleRepository;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                this._logger.LogInformation("Chargement de la sidebar : récupération de la liste des styles.");
                var styles = _styleRepository.FindAll().ToList();

                this._logger.LogDebug(
                    "Chargement de la sidebar réussi. Nombre de styles récupérés : {NombreStyles}.",
                    styles.Count);
                return View(styles);
            }
            catch (Exception ex)
            {
                this._logger.LogError(
                    ex,
                    "Erreur lors du chargement de la sidebar et de la récupération des styles.");

                return View(Enumerable.Empty<Style>());
            }
        }

    }
}
