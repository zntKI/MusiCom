using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.Event;
using MusiCom.Infrastructure.Data.Entities;
using MusiCom.Infrastructure.Data.Entities.Events;

namespace MusiCom.Core.Contracts
{
    public interface IEventService
    {
        /// <summary>
        /// Seeds the given Event in the Database
        /// </summary>
        /// <param name="model">Model passed by the View</param>
        /// <param name="artist">Artist who have created the Event</param>
        /// <param name="image">Image File passed by the View</param>
        Task CreateEventAsync(EventAddViewModel model, ApplicationUser artist, IFormFile image);

        /// <summary>
        /// Gets all Events which correspond to the given criteria
        /// </summary>
        /// <param name="genre">Genre Name if passed by the View</param>
        /// <param name="searchTerm">Word or Phrase which will be searched either in the Event Title or the Event's Creator</param>
        /// <param name="currentPage">Current Page of all which hold Events</param>
        /// <param name="eventsPerPage">Number of Events that could be held in a Single Page</param>
        /// <returns>Model which will be used for the Visualisation in the View</returns>
        Task<EventQueryServiceModel> GetAllEventsAsync(
            string? genre = null,
            string? searchTerm = null,
            int currentPage = 1,
            int eventsPerPage = 1);

        /// <summary>
        /// Gets the Event with the given Id from the Database
        /// </summary>
        /// <param name="id">The Id of the Event</param>
        /// <returns></returns>
        Task<Event> GetEventByIdAsync(Guid id);

        /// <summary>
        /// Gets the wanted Event
        /// </summary>
        /// <param name="entity">Event</param>
        /// <returns>EvemtDetailsViewModel</returns>
        Task<EventDetailsViewModel> GetEventByIdForDetailsAsync(Event entity);

        Task<IEnumerable<EventPost>> GetAllPostsForEventAsync(Guid id);

        /// <summary>
        /// Marks the Given Event as Deleted
        /// </summary>
        /// <param name="eventt">The Event to be marked as Deleted</param>
        Task DeleteEventAsync(Event eventt);

        /// <summary>
        /// Edits a given Event
        /// </summary>
        /// <param name="eventt">The Event to be Edited</param>
        /// <param name="model">The Model which contains the New Data for the Event</param>
        /// <param name="image">The new Image file if there is such</param>
        Task EditEventAsync(Event eventt, EventEditViewModel model, IFormFile image);
    }
}
