using Microsoft.AspNetCore.Mvc;
using Webzine.WebApplication.ViewModels;

namespace Webzine.WebApplication.Controllers
{
    /// <summary>
    /// Represents a controller responsible for handling error-related requests and returning appropriate error views based on HTTP status codes. This controller provides a centralized way to manage and display error messages for different types of errors that may occur within the web application.
    /// </summary>
    public class ErrorController : Controller
    {
        /// <summary>
        /// Retourne la vue d'erreur correspondante au code d'état HTTP fourni. Gère les erreurs 404 (Page introuvable) et 400 (Requête invalide) avec des messages personnalisés, et affiche un message générique pour les autres codes d'erreur.
        /// </summary>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public IActionResult Index(int statusCode)
        {
            var model = new ErrorViewModel
            {
                StatusCode = statusCode,
                Title = "Error inattendue",
                Message = "Une erreur inattendue s'est produite. Veuillez réessayer plus tard."
            };

            if (statusCode == 404)
            {
                model.Title = "Page introuvable";
                model.Message = "La ressource que vous recherchez n'existe pas ou a été supprimée.";
            }
            else if (statusCode == 400)
            {
                model.Title = "Requête invalide";
                model.Message = "La requête envoyée au serveur est incorrecte ou n'a pas pu être traitée.";
            }

            return this.View(model);
        }
    }
}
