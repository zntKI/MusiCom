using MusiCom.Core.Models.Genre;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Contracts.Admin
{
    /// <summary>
    /// Interface for GenreService
    /// </summary>
    public interface IGenreService
    {
        Task CreateGenreAsync(GenreViewModel model);

        IEnumerable<GenreAllViewModel> GetAllGenres();

        Task<IEnumerable<string>> GetAllGenreNames();

        Task<Genre> GetGenreByIdAsync(Guid Id);

        Task EditGenreAsync(Guid id, GenreAllViewModel model);

        Task DeleteGenreAsync(Guid id);
    }
}
