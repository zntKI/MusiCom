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
        Task<IEnumerable<GenreAllViewModel>> GetAllGenresAsync();

        /// <summary>
        /// Gets All Genres' Names
        /// </summary>
        /// <returns>A Collection of Genre Names</returns>
        Task<IEnumerable<string>> GetAllGenreNamesAsync();

        /// <summary>
        /// Gets the Genre with the given Id
        /// </summary>
        /// <param name="Id">The Id of a Genre</param>
        /// <returns>The desired Genre</returns>
        Task<Genre> GetGenreByIdAsync(Guid Id);

        /// <summary>
        /// Edits a Genre with a given Id
        /// </summary>
        /// <param name="id">Id of the Genre</param>
        /// <param name="model">Model which is passed from the View</param>
        Task EditGenreAsync(Guid id, GenreAllViewModel model);

        /// <summary>
        /// Marks a given Genre as Deleted
        /// </summary>
        /// <param name="id">The Id of the given Genre</param>
        Task DeleteGenreAsync(Guid id);
    }
}
