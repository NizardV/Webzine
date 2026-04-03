using Microsoft.AspNetCore.Mvc;

namespace Webzine.WebApplication.Controllers
{
    /// <summary>
    /// Represents a controller responsible for handling contact-related requests and returning the appropriate contact view. This controller provides a way for users to access the contact page of the web application, where they can find information on how to get in touch with the organization or submit inquiries. The ContactController serves as a central point for managing contact-related interactions within the web application.
    /// </summary>
    public class ContactController : Controller
    {
        /// <summary>
        /// Retourne la page contact.
        /// </summary>
        /// <returns>Une vue.</returns>
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
