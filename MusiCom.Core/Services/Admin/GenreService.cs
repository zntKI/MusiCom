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

        /// <summary>
        /// Creates a Genre
        /// </summary>
        /// <param name="model">model passed by the controller</param>
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

        /// <summary>
        /// Marks a given Genre as Deleted
        /// </summary>
        /// <param name="id">The Id of the given Genre</param>
        public async Task DeleteGenreAsync(Guid id)
        {
            Genre genre = await GetGenreByIdAsync(id);
            genre.IsDeleted = true;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Edits a Genre with a given Id
        /// </summary>
        /// <param name="id">Id of the Genre</param>
        /// <param name="model">Model which is passed from the View</param>
        public async Task EditGenreAsync(Guid id, GenreAllViewModel model)
        {
            var genre = await GetGenreByIdAsync(id);

            genre.Name = model.Name;
            genre.DateOfCreation = DateTime.Now;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Gets All Genres' Names
        /// </summary>
        /// <returns>A Collection of Genre Names</returns>
        public async Task<IEnumerable<string>> GetAllGenreNames()
        {
            return await repo.AllReadonly<Genre>()
                .Where(g => g.IsDeleted == false)
                .Select(g => g.Name)
                .ToListAsync();
        }

        /// <summary>
        /// Lists all Genres ordered by DateOfCreation
        /// </summary>
        /// <returns>All Genres</returns>
        //TODO:
        public IEnumerable<GenreAllViewModel> GetAllGenres()
        {
            var models = repo.All<Genre>()
                .Where(g => g.IsDeleted == false)
                .Select(g => new GenreAllViewModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    DateOfCreation = g.DateOfCreation
                })
                .OrderByDescending(g => g.DateOfCreation);

            return models;
        }

        /// <summary>
        /// Gets the Genre with the given Id
        /// </summary>
        /// <param name="id">The Id of a Genre</param>
        /// <returns>The desired Genre</returns>
        public async Task<Genre> GetGenreByIdAsync(Guid id)
        {
            return await repo.GetByIdAsync<Genre>(id);
        }
    }
}
