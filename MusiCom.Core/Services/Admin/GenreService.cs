using Microsoft.EntityFrameworkCore;
using MusiCom.Core.Contracts.Admin;
using MusiCom.Core.Models.Genre;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Services.Admin
{
    /// <summary>
    /// Contains the logic for Genre functionalities
    /// </summary>
    public class GenreService : IGenreService
    {
        private readonly IRepository repo;

        public GenreService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task CreateGenreAsync(GenreViewModel model)
        {
            var genre = new Genre()
            {
                Name = model.Name,
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

            await repo.SaveChangesAsync();
        }

        public async Task EditGenreAsync(Guid id, GenreAllViewModel model)
        {
            var genre = await GetGenreByIdAsync(id);

            genre.Name = model.Name;
            genre.DateOfCreation = DateTime.Now;

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
