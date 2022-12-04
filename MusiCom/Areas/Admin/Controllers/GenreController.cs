using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Genre;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Areas.Admin.Controllers
{
    /// <summary>
    /// Contains functionalities regarding the genres
    /// </summary>
    public class GenreController : AdminController
    {
        private readonly IGenreService genreService;

        public GenreController(IGenreService _genreService)
        {
            genreService = _genreService;
        }

        /// <summary>
        /// Renders a view which shows all genres
        /// </summary>
        /// <returns>A View</returns>
        [HttpGet]
        public IActionResult All()
        {
            var genres = genreService.GetAllGenres();

            return View(genres);
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

            return RedirectToAction("All", "Genre");
        }

        /// <summary>
        /// Finds the Genre with the given Id and then renders a View
        /// </summary>
        /// <param name="id">The Id of the Genre which is about to be Edited</param>
        /// <returns>A View</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Genre genre = await genreService.GetGenreByIdAsync(id);

            //TODO:
            if (genre == null)
            {
                return BadRequest();
            }

            GenreAllViewModel model = new GenreAllViewModel()
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return View(model);
        }

        /// <summary>
        /// Edits the Genre
        /// </summary>
        /// <param name="id">The Id of the Genre which is Edited</param>
        /// <param name="model">The Genre</param>
        /// <returns>Redirects to All Action</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, GenreAllViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await genreService.EditGenreAsync(id, model);

            return RedirectToAction("All", "Genre");
        }

        /// <summary>
        /// Marks the given Genre as Deleted
        /// </summary>
        /// <param name="id">The Id of the given Genre</param>
        /// <returns>Redirects to All Action</returns>
        public async Task<IActionResult> Remove(Guid id)
        {
            //TODO:
            if ((await genreService.GetGenreByIdAsync(id)) == null)
            {
                return BadRequest();
            }

            await genreService.DeleteGenreAsync(id);

            return RedirectToAction("All", "Genre");
        }
    }
}
