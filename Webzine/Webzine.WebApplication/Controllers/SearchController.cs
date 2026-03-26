using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.WebApplication.ViewModels;

namespace Webzine.WebApplication.Controllers
{
    public class SearchController : Controller
    {
        [HttpGet]
        public IActionResult Index(string? q)
        {
            q = q?.Trim();

            var model = new SearchResultsModel
            {
                Query = q ?? string.Empty,
                Artistes = new(),
                Titres = new(),
            };

            if (string.IsNullOrWhiteSpace(q))
            {
                return View(model);
            }

            model.Artistes = // Ajouter la logique pour recupérer à partir du repo
                .Where(a => !string.IsNullOrWhiteSpace(a.Nom) && a.Nom.Contains(q, StringComparison.OrdinalIgnoreCase))
                .ToList();

            model.Titres = // Ajouter la logique pour recupérer à partir du repo
                .Where(t =>
                    (!string.IsNullOrWhiteSpace(t.Libelle) && t.Libelle.Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrWhiteSpace(t.Artiste?.Nom) && t.Artiste.Nom.Contains(q, StringComparison.OrdinalIgnoreCase)))
                .ToList();

            return View(model);
        }
    }
}
