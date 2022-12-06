using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.Event;
using MusiCom.Infrastructure.Data.Entities;
using MusiCom.Infrastructure.Data.Entities.Events;

namespace MusiCom.Core.Contracts
{
    public interface IEventService
    {
        Task CreateEventAsync(EventAddViewModel model, ApplicationUser artist, IFormFile image);

        Task<EventQueryServiceModel> GetAllEventsAsync(
            string? genre = null,
            string? searchTerm = null,
            int currentPage = 1,
            int eventsPerPage = 1);

        Task<Event> GetEventByIdAsync(Guid id);

        Task<EventDetailsViewModel> GetEventByIdForDetailsAsync(Guid id);

        Task<IEnumerable<EventPost>> GetAllPostsForEventAsync(Guid id);

        Task DeleteEventAsync(Event eventt);

        Task EditEventAsync(Event eventt, EventEditViewModel model, IFormFile image);
    }
}
