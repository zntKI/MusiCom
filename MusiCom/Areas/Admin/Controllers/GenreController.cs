using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Constants;
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
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var genres = await genreService.GetAllGenres();

            return View(genres);
        }

        /// <summary>
        /// Renders the view for add functionality
        /// </summary>
        [HttpGet]
        public IActionResult Add()
        { 
            GenreViewModel model = new GenreViewModel();

            return View(model);
        }

        /// <summary>
        /// Adds a genre
        /// </summary>
        /// <param name="model">Receives the Genre Model from the Client</param>
        /// <returns>Redirects to Action All</returns>
        [HttpPost]
        public async Task<IActionResult> Add(GenreViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await genreService.CreateGenreAsync(model);

            return RedirectToAction("All");
        }

        /// <summary>
        /// Finds the Genre with the given Id
        /// </summary>
        /// <param name="id">Id of the Genre which is about to be Edited</param>
        /// <returns>View for Editing the Genre</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            Genre genre = await genreService.GetGenreByIdAsync(id);

            if (genre == null)
            {
                TempData[MessageConstant.ErrorMessage] = "There is no such Genre!";

                return RedirectToAction("All");
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
        /// <param name="id">Id of the Genre which is Edited</param>
        /// <param name="model">The Genre Model containing the new Data</param>
        /// <returns>Redirects to Action All</returns>
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, GenreAllViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await genreService.EditGenreAsync(id, model);

            TempData[MessageConstant.SuccessMessage] = "Successfully edited Genre";

            return RedirectToAction("All");
        }

        /// <summary>
        /// Marks the given Genre as Deleted
        /// </summary>
        /// <param name="id">Id of the given Genre</param>
        /// <returns>Redirects to Action All</returns>
        [HttpPost]
        public async Task<IActionResult> Remove(Guid id)
        {
            if ((await genreService.GetGenreByIdAsync(id)) == null)
            {
                TempData[MessageConstant.ErrorMessage] = "There is no such Genre";

                return RedirectToAction("All");
            }

            await genreService.DeleteGenreAsync(id);

            TempData[MessageConstant.SuccessMessage] = "Successfully added Genre";

            return RedirectToAction("All");
        }
    }
}
