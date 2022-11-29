using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.Event;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Core.Contracts
{
    public interface IEventService
    {
        Task CreateEventAsync(EventAddViewModel model, ApplicationUser artist, IFormFile image);

        Task<EventQueryServiceModel> GetAllEvents(
            string? genre = null,
            string? searchTerm = null,
            int currentPage = 1,
            int eventsPerPage = 1);

        Task<EventDetailsViewModel> GetEventById(Guid id);
    }
}
