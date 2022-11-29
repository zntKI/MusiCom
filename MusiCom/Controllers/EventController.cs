using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Event;
using MusiCom.Infrastructure.Data.Entities;
using System.Security.Claims;

namespace MusiCom.Controllers
{
    [Authorize]
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
            var queryResult = await eventService.GetAllEvents(
                query.Genre,
                query.SearchTerm,
                query.CurrentPage,
                EventAllQueryModel.EventsPerPage);

            query.TotalEventsCount = queryResult.TotalEventsCount;
            query.Events = queryResult.Events;

            var eventGenres = await genreService.GetAllGenreNames();
            query.Genres = eventGenres;

            return View(query);
        }

        /// <summary>
        /// Generates the EventViewModel for Add and Passes it to the View
        /// </summary>
        /// <returns>The View for Adding an Event</returns>
        [HttpGet]
        public IActionResult Add()
        {
            EventAddViewModel model = new EventAddViewModel()
            { 
                Genres = genreService.GetAllGenres()
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
        public async Task<IActionResult> Add(EventAddViewModel model, IFormFile image)
        {
            var artist = await userManager.GetUserAsync(User);

            //if (!User.IsInRole("Artist"))
            //{
            //    throw new InvalidOperationException();
            //}

            ModelState.Remove("Image");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await eventService.CreateEventAsync(model, artist, image);
            }
            catch (Exception)
            {

                throw;
            }

            return RedirectToAction("All", "Event");
        }

        /// <summary>
        /// Renders the Details Page for an Event
        /// </summary>
        /// <param name="Id">Id of the Given Event</param>
        /// <returns>A View</returns>
        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var eventt = await eventService.GetEventById(Id);

            return View(eventt);
        }
    }
}
