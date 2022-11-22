using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.New;

namespace MusiCom.Core.Contracts
{
    /// <summary>
    /// Interface for GenreService
    /// </summary>
    public interface INewServices
    {
        Task CreateNewAsync(Guid userId, NewAddViewModel model, IFormFile titlePhoto);

        IEnumerable<NewAllViewModel> GetLastThreeNewsAsync();
    }
}
