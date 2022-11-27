using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.Event;

namespace MusiCom.Core.Contracts
{
    public interface IEventService
    {
        Task CreateEventAsync(EventAddViewModel model, Guid artistId, IFormFile image);

        Task<EventQueryServiceModel> GetAllEvents(
            string? genre = null,
            string? searchTerm = null,
            int currentPage = 1,
            int eventsPerPage = 1);
    }
}
