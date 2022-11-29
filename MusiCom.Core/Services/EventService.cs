using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Event;
using MusiCom.Infrastructure.Data.Entities.News;
using MusiCom.Infrastructure.Data.Entities.Events;
using Microsoft.AspNetCore.Http;
using MusiCom.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using MusiCom.Infrastructure.Data.Entities;

namespace MusiCom.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IRepository repo;

        public EventService(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Seeds the given Event in the Database
        /// </summary>
        /// <param name="model">Model passed by the View</param>
        /// <param name="artistId">Artist who have created the Event</param>
        /// <param name="image">Image File passed by the View</param>
        /// <exception cref="InvalidOperationException">Throws if something goes wrong</exception>
        public async Task CreateEventAsync(EventAddViewModel model, ApplicationUser artist, IFormFile image)
        {
            Event eventt = new Event()
            {
                Title = model.Title,
                Description = model.Description,
                GenreId = model.GenreId,
                ArtistId = artist.Id,
                Artist = artist,
                Date = model.Date,
                IsDeleted = false,
                DateOfCreation = DateTime.Now
            };

            string type = image.ContentType;

            //TODO: Fix
            if (!type.Contains("image"))
            {
                throw new InvalidOperationException();
            }

            string contentType = type.Substring(type.IndexOf('/') + 1, type.Length - type.Substring(0, type.IndexOf('/')).Length - 1);

            if (contentType != "png" && contentType != "jpeg" && contentType != "jpg")
            {
                throw new InvalidOperationException("Please import an image in one of the formats shown above!");
            }

            //TODO: Fix
            if (image.Length > 0)
            {
                using var stream = new MemoryStream();
                await image.CopyToAsync(stream);
                eventt.Image = stream.ToArray();
            }
            else
            {
                throw new InvalidOperationException();
            }

            await repo.AddAsync(eventt);
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all Events which correspond to the given criteria
        /// </summary>
        /// <param name="genre">Genre Name if passed by the View</param>
        /// <param name="searchTerm">Word or Phrase which will be searched either in the Event Title or the Event's Creator</param>
        /// <param name="currentPage">The Current Page of all which hold Events</param>
        /// <param name="eventsPerPage">The Number of Events that could be held in a Single Page</param>
        /// <returns>Model which will be used for the Visualisation in the View</returns>
        public async Task<EventQueryServiceModel> GetAllEvents(string? genre = null, string? searchTerm = null, int currentPage = 1, int eventsPerPage = 1)
        {
            var eventsQuery = repo.AllReadonly<Event>()
                .Where(e => e.IsDeleted == false);

            if (!String.IsNullOrWhiteSpace(genre))
            {
                eventsQuery = repo.AllReadonly<Event>()
                    .Where(e => e.Genre.Name == genre);
            }

            if (!String.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                eventsQuery = eventsQuery
                    .Where(e => EF.Functions.Like(e.Title.ToLower(), searchTerm) ||
                        EF.Functions.Like(e.Artist.UserName.ToLower(), searchTerm));
            }

            var events = await eventsQuery
                .Skip((currentPage - 1) * eventsPerPage)
                .Take(eventsPerPage)
                .Select(e => new EventAllViewModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Image = e.Image,
                    Date = e.Date,
                    Description = e.Description,
                    ArtistName = e.Artist.UserName,
                })
                .OrderBy(e => e.Date)
                .ToListAsync();

            var totalEvents = await eventsQuery.CountAsync();

            return new EventQueryServiceModel()
            {
                TotalEventsCount = totalEvents,
                Events = events
            };
        }

        /// <summary>
        /// Gets all Posts attached to a Given Event
        /// </summary>
        /// <param name="id">Id of the Event</param>
        /// <returns>Collection of Posts</returns>
        public async Task<IEnumerable<EventPost>> GetAllPostsForEvent(Guid id)
        {
            return await repo.All<EventPost>(ep => ep.EventId == id)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the wanted Event
        /// </summary>
        /// <param name="id">Event Id</param>
        /// <returns>EvemtDetailsViewModel</returns>
        public async Task<EventDetailsViewModel> GetEventById(Guid id)
        {
            //TODO: Fix
            var entity = await repo.GetByIdAsync<Event>(id);
            var genre = await repo.GetByIdAsync<Genre>(entity.GenreId);
            var artist = await repo.GetByIdAsync<ApplicationUser>(entity.ArtistId);

            EventDetailsViewModel model = new EventDetailsViewModel()
            {
                Id = id,
                Image = entity.Image,
                Title = entity.Title,
                Description = entity.Description,
                Date = entity.Date,
                ArtistName = artist.UserName,
                Genre = genre,
                EventPosts = await GetAllPostsForEvent(entity.Id),
            };

            return model;
        }
    }
}
