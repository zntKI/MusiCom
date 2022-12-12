using Ganss.Xss;
using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Genre;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.Events;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Services.Admin
{
    /// <summary>
    /// Contains the logic for Genre functionalities
    /// </summary>
    public class GenreService : IGenreService
    {
        private readonly IRepository repo;
        private HtmlSanitizer sanitizer;

        public GenreService(IRepository _repo)
        {
            repo = _repo;
            sanitizer = new HtmlSanitizer();
        }

        public async Task CreateGenreAsync(GenreViewModel model)
        {
            var genre = new Genre()
            {
                Name = sanitizer.Sanitize(model.Name),
                DateOfCreation = DateTime.Now,
                IsDeleted = false
            };

            await repo.AddAsync(genre);
            await repo.SaveChangesAsync();
        }

        public async Task DeleteGenreAsync(Guid id)
        {
            Genre genre = await GetGenreByIdAsync(id);
            genre.IsDeleted = true;

            var news = await repo.All<New>().Where(e => !e.IsDeleted && e.GenreId == id).ToListAsync();
            foreach (var neww in news)
            {
                neww.IsDeleted = true;
            }

            var events = await repo.All<Event>().Where(e => !e.IsDeleted && e.GenreId == id).ToListAsync();
            foreach (var eventt in events)
            {
                eventt.IsDeleted = true;
            }

            await repo.SaveChangesAsync();
        }

        public async Task EditGenreAsync(Guid id, GenreAllViewModel model)
        {
            var genre = await GetGenreByIdAsync(id);

            genre.Name = sanitizer.Sanitize(model.Name);

            await repo.SaveChangesAsync();
        }

        public async Task<IEnumerable<string>> GetAllGenreNamesAsync()
        {
            return await repo.AllReadonly<Genre>()
                .Where(g => g.IsDeleted == false)
                .Select(g => g.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<GenreAllViewModel>> GetAllGenresAsync()
        {
            var models = await repo.AllReadonly<Genre>()
                .Where(g => g.IsDeleted == false)
                .OrderByDescending(g => g.DateOfCreation)
                .Select(g => new GenreAllViewModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    DateOfCreation = g.DateOfCreation
                })
                .ToListAsync();

            return models;
        }

        public async Task<Genre> GetGenreByIdAsync(Guid id)
        {
            return await repo.GetByIdAsync<Genre>(id);
        }
    }
}
