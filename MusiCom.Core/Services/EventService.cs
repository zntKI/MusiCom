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

            await AddImage(eventt, image);

            await repo.AddAsync(eventt);
            await repo.SaveChangesAsync();
        }

        public async Task DeleteEventAsync(Event eventt)
        {
            eventt.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        public async Task EditEventAsync(Event eventt, EventEditViewModel model, IFormFile image)
        {
            eventt.Title = model.Title;
            eventt.Description = model.Description;
            eventt.GenreId = model.GenreId;

            if (model.Date != null)
            {
                eventt.Date = (DateTime)model.Date;
            }

            if (image != null)
            {
                await AddImage(eventt, image);
            }

            await repo.SaveChangesAsync();
        }

        public async Task<EventQueryServiceModel> GetAllEventsAsync(string? genre = null, string? searchTerm = null, int currentPage = 1, int eventsPerPage = 1)
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
                .OrderBy(e => e.Date)
                .Skip((currentPage - 1) * eventsPerPage)
                .Take(eventsPerPage)
                .Select(e => new EventAllViewModel
                {
                    Id = e.Id,
                    Title = e.Title,
                    Image = e.Image,
                    Date = e.Date,
                    Description = e.Description,
                    Artistt = new EventAllArtistViewModel()
                    { 
                        Id = e.ArtistId,
                        ArtistName = e.Artist.UserName
                    }
                })
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
        public async Task<IEnumerable<EventPost>> GetAllPostsForEventAsync(Guid id)
        {
            return await repo.All<EventPost>(ep => ep.EventId == id)
                .ToListAsync();
        }

        public async Task<Event> GetEventByIdAsync(Guid id)
        {
            return await repo.GetByIdAsync<Event>(id);
        }

        public async Task<EventDetailsViewModel> GetEventByIdForDetailsAsync(Guid id)
        {
            var entity = await repo.GetByIdAsync<Event>(id);
            var artist = await repo.GetByIdAsync<ApplicationUser>(entity.ArtistId);

            EventDetailsViewModel model = new EventDetailsViewModel()
            {
                Id = id,
                Image = entity.Image,
                Title = entity.Title,
                Description = entity.Description,
                Date = entity.Date,
                ArtistName = artist.UserName,
                Genre = await repo.GetByIdAsync<Genre>(entity.GenreId),
                EventPosts = await GetAllPostsForEventAsync(entity.Id),
            };

            return model;
        }

        /// <summary>
        /// Adds the given Image to the Event
        /// </summary>
        /// <param name="eventt">The Event</param>
        /// <param name="image">The Image</param>
        /// <returns>The modified Event</returns>
        /// <exception cref="InvalidOperationException">passed to the controller</exception>
        public async Task<Event> AddImage(Event eventt, IFormFile image)
        {
            string type = image.ContentType;

            if (!type.Contains("image"))
            {
                throw new InvalidOperationException("Not an image");
            }

            string contentType = type.Substring(type.IndexOf('/') + 1, type.Length - type.Substring(0, type.IndexOf('/')).Length - 1);

            if (contentType != "png" && contentType != "jpeg" && contentType != "jpg")
            {
                throw new InvalidOperationException("Not the right image format");
            }

            if (image.Length > 0)
            {
                using var stream = new MemoryStream();
                await image.CopyToAsync(stream);
                eventt.Image = stream.ToArray();
            }
            else
            {
                throw new InvalidOperationException("Image else");
            }

            return eventt;
        }
    }
}
