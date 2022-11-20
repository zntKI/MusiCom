using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Genre;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Services
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
        /// Lists all Genres ordered by DateOfCreation
        /// </summary>
        /// <returns>All Genres</returns>
        //TODO:
        public IEnumerable<GenreAllViewModel> GetAllGenres()
        {
            List<GenreAllViewModel> models = new List<GenreAllViewModel>();

            var entities = repo.All<Genre>();

            foreach (var entity in entities)
            {
                if (entity.IsDeleted == true)
                {
                    continue;
                }

                var model = new GenreAllViewModel()
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    DateOfCreation = entity.DateOfCreation,
                };

                models.Add(model);
            }

            return models.OrderByDescending(m => m.DateOfCreation);
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
