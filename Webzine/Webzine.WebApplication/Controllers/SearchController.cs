using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.WebApplication.ViewModels;
using Webzine.Repository.Contracts;

namespace Webzine.WebApplication.Controllers
{
    public class SearchController : Controller
    {
        public IArtisteRepository _artisteRepository; 
        public ITitreRepository _titreRepository;

        public SearchController(IArtisteRepository artisteRepository, ITitreRepository titreRepository)
        {
            _artisteRepository = artisteRepository;
            _titreRepository = titreRepository;
        }

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

            model.Artistes = _artisteRepository.FindAll()
                .Where(a => !string.IsNullOrWhiteSpace(a.Nom) && a.Nom.Contains(q, StringComparison.OrdinalIgnoreCase))
                .ToList();

            model.Titres = _titreRepository.FindAll()
                .Where(t =>
                    (!string.IsNullOrWhiteSpace(t.Libelle) && t.Libelle.Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrWhiteSpace(t.Artiste?.Nom) && t.Artiste.Nom.Contains(q, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            return View(model);
        }
    }
}
