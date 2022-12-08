using MusiCom.Core.Models.Genre;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Contracts.Admin
{
    /// <summary>
    /// Interface for GenreService
    /// </summary>
    public interface IGenreService
    {
        /// <summary>
        /// Creates a Genre
        /// </summary>
        /// <param name="model">Model passed by the controller</param>
        Task CreateGenreAsync(GenreViewModel model);

        /// <summary>
        /// Lists all Genres ordered by DateOfCreation
        /// </summary>
        /// <returns>All Genres</returns>
        Task<IEnumerable<GenreAllViewModel>> GetAllGenres();

        Task<IEnumerable<string>> GetAllGenreNames();

        Task<Genre> GetGenreByIdAsync(Guid Id);

        /// <summary>
        /// Edits a Genre with a given Id
        /// </summary>
        /// <param name="id">Id of the Genre</param>
        /// <param name="model">Model which is passed from the View</param>
        Task EditGenreAsync(Guid id, GenreAllViewModel model);

        Task DeleteGenreAsync(Guid id);
    }
}
