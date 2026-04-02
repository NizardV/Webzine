using Microsoft.AspNetCore.Mvc;
using Webzine.Entity.Fixtures;
using Webzine.WebApplication.ViewModels;
using Webzine.Repository.Contracts;

namespace Webzine.WebApplication.Controllers
{
    /// <summary>
    /// Represents a controller responsible for handling search-related requests and returning the appropriate search results view. This controller provides functionality for users to search for artists and titles within the web application, allowing them to find relevant information based on their search queries. The SearchController serves as a central point for managing search interactions and displaying the corresponding results to users.
    /// </summary>
    public class SearchController : Controller
    {
        public IArtisteRepository _artisteRepository; 
        public ITitreRepository _titreRepository;

        /// <summary>
        /// Represents a constructor for the SearchController class that initializes the controller with the necessary repositories for accessing artist and title data. The constructor takes two parameters: an IArtisteRepository for managing artist-related data and an ITitreRepository for managing title-related data. These repositories are essential for performing search operations and retrieving relevant information based on user queries. By injecting these dependencies through the constructor, the SearchController can effectively handle search requests and provide accurate results to users.
        /// </summary>
        /// <param name="artisteRepository"></param>
        /// <param name="titreRepository"></param>
        public SearchController(IArtisteRepository artisteRepository, ITitreRepository titreRepository)
        {
            this._artisteRepository = artisteRepository;
            this._titreRepository = titreRepository;
        }

        /// <summary>
        /// Returns the search results view based on the provided search query. The method takes an optional string parameter 'q' representing the search query entered by the user. It processes the query, retrieves relevant artists and titles from the repositories, and constructs a SearchResultsModel to be passed to the view for rendering. If the search query is empty or null, it simply returns the view with an empty model.
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
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
                return this.View(model);
            }

            model.Artistes = this._artisteRepository.FindAll()
                .Where(a => !string.IsNullOrWhiteSpace(a.Nom) && a.Nom.Contains(q, StringComparison.OrdinalIgnoreCase))
                .ToList();

            model.Titres = this._titreRepository.FindAll()
                .Where(t =>
                    (!string.IsNullOrWhiteSpace(t.Libelle) && t.Libelle.Contains(q, StringComparison.OrdinalIgnoreCase))
                    || (!string.IsNullOrWhiteSpace(t.Artiste?.Nom) && t.Artiste.Nom.Contains(q, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            return this.View(model);
        }
    }
}
