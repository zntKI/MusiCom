using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Genre;

namespace MusiCom.Controllers
{
    /// <summary>
    /// Contains functionalities regarding the genres
    /// </summary>
    public class GenreController : Controller
    {
        private readonly IGenreService genreService;

        public GenreController(IGenreService _houseService)
        {
            genreService = _houseService;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Renders the view for add functionality
        /// </summary>
        /// <returns>AddView</returns>
        [HttpGet]
        public IActionResult Add()
        { 
            GenreViewModel model = new GenreViewModel();

            return View(model);
        }

        /// <summary>
        /// Adds a genre
        /// </summary>
        /// <param name="model">receives the genre model from the client</param>
        /// <returns>Redirects to Home page</returns>
        [HttpPost]
        public async Task<IActionResult> Add(GenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await genreService.CreateGenreAsync(model);

            return RedirectToAction("Index", "Home");
        }
    }
}
