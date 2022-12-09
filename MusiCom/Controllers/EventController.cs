using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Constants;
using MusiCom.Core.Contracts;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Event;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Controllers
{
    public class EventController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IEventService eventService;
        private readonly IGenreService genreService;

        public EventController(IEventService _eventService, IGenreService _genreService,
                                UserManager<ApplicationUser> _userManager)
        {
            eventService = _eventService;
            genreService = _genreService;
            userManager = _userManager;
        }

        /// <summary>
        /// Shows All Events or Events which correspond to a given criteria
        /// </summary>
        /// <param name="query">The sorting parameters passed by the Url</param>
        /// <returns>View with the applied sorting</returns>
        [HttpGet]
        public async Task<IActionResult> All([FromQuery] EventAllQueryModel query)
        {
            var queryResult = await eventService.GetAllEventsAsync(
                query.Genre,
                query.SearchTerm,
                query.CurrentPage,
                EventAllQueryModel.EventsPerPage);

            query.TotalEventsCount = queryResult.TotalEventsCount;
            query.Events = queryResult.Events;

            var eventGenres = await genreService.GetAllGenreNamesAsync();
            query.Genres = eventGenres;

            return View(query);
        }

        /// <summary>
        /// Generates the EventViewModel for Add and Passes it to the View
        /// </summary>
        /// <returns>The View for Adding an Event</returns>
        [HttpGet]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Add()
        {
            EventAddViewModel model = new EventAddViewModel()
            { 
                Genres = await genreService.GetAllGenresAsync()
            };

            return View(model);
        }

        /// <summary>
        /// Checks the Model for validity and Calls the Service method which will Create the Event
        /// </summary>
        /// <param name="model">Model passed by the View</param>
        /// <param name="image">Image File, inserted when Creating the Event</param>
        /// <returns>Redirects to All Events</returns>
        [HttpPost]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Add(EventAddViewModel model, IFormFile image)
        {
            var artist = await userManager.GetUserAsync(User);

            ModelState.Remove("Image");
            if (!ModelState.IsValid || image == null)
            {
                model.Genres = await genreService.GetAllGenresAsync();
                return View(model);
            }

            try
            {
                await eventService.CreateEventAsync(model, artist, image);
            }
            catch (Exception e)
            {
                if (e.Message == "Not an image")
                {
                    TempData[MessageConstant.ErrorMessage] = "Please insert an image";
                }
                else if (e.Message == "Not the right image format")
                {
                    TempData[MessageConstant.ErrorMessage] = "Please insert an image with one of the formats shown";
                }
                else if (e.Message == "Image else")
                {
                    TempData[MessageConstant.WarningMessage] = "An Error occured";
                }
                model.Genres = await genreService.GetAllGenresAsync();
                return View(model);
            }

            return RedirectToAction("All");
        }

        /// <summary>
        /// Renders the Details Page for an Event
        /// </summary>
        /// <param name="Id">Id of the Given Event</param>
        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var eventt = await eventService.GetEventByIdForDetailsAsync(Id);

            return View(eventt);
        }

        /// <summary>
        /// Deletes the Event with the given Id
        /// </summary>
        /// <param name="Id">Id of the Event</param>
        /// <returns>Redirects to Action All</returns>
        /// <exception cref="InvalidOperationException"></exception>
        [HttpPost]
        [Authorize(Roles = "Artist, Admin")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var user = await userManager.GetUserAsync(User);
            var eventt = await eventService.GetEventByIdAsync(Id);

            if (eventt == null)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            if (user.Id != eventt.ArtistId)
            {
                TempData[MessageConstant.ErrorMessage] = "Can't delete other Artist's Events";
                return RedirectToAction("All");
            }

            try
            {
                await eventService.DeleteEventAsync(eventt);
            }
            catch (Exception)
            {
                TempData[MessageConstant.WarningMessage] = "An Error occured";
            }

            return RedirectToAction("All");
        }

        /// <summary>
        /// Renders a View for Editing an Event
        /// </summary>
        /// <param name="Id">Id of the Event</param>
        /// <returns>View for Editing</returns>
        [HttpGet]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Edit(Guid Id)
        {
            var user = await userManager.GetUserAsync(User);
            var eventt = await eventService.GetEventByIdAsync(Id);

            if (eventt == null)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            if (user.Id != eventt.ArtistId)
            {
                TempData[MessageConstant.ErrorMessage] = "Can't edit other Artist's Events";
                return RedirectToAction("All");
            }

            var model = new EventEditViewModel()
            {
                Id = eventt.Id,
                Title = eventt.Title,
                Description = eventt.Description,
                Image = eventt.Image,
                Date = eventt.Date,
                Genres = await genreService.GetAllGenresAsync(),
                ArtistId = eventt.ArtistId,
                GenreId = eventt.GenreId
            };

            return View(model);
        }

        /// <summary>
        /// Edits an Event
        /// </summary>
        /// <param name="Id">Id of the Event</param>
        /// <param name="model">Edited Data for the Event</param>
        /// <param name="image">New's Image if there is such</param>
        /// <returns>Redirects to Action All</returns>
        [HttpPost]
        [Authorize(Roles = "Artist")]
        public async Task<IActionResult> Edit(Guid Id, EventEditViewModel model, IFormFile image)
        {
            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var eventt = await eventService.GetEventByIdAsync(Id);

            if (eventt == null)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            if (Id != model.Id)
            {
                TempData[MessageConstant.ErrorMessage] = "Not found";
                return RedirectToAction("All");
            }

            try
            {
                await eventService.EditEventAsync(eventt, model, image);
            }
            catch (Exception e)
            {
                if (e.Message == "Not an image")
                {
                    TempData[MessageConstant.ErrorMessage] = "Please insert an image";
                }
                else if (e.Message == "Not the right image format")
                {
                    TempData[MessageConstant.ErrorMessage] = "Please insert an image with one of the formats shown";
                }
                else if (e.Message == "Image else")
                {
                    TempData[MessageConstant.WarningMessage] = "An Error occured";
                }
            }

            return RedirectToAction("All");
        }
    }
}
