using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.New;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Contracts
{
    /// <summary>
    /// Interface for GenreService
    /// </summary>
    public interface INewServices
    {
        Task CreateNewAsync(Guid userId, NewAddViewModel model, IFormFile titlePhoto);

        IEnumerable<NewAllViewModel> GetLastThreeNewsAsync();

        Task<New> GetNewByIdAsync(Guid newId);

        ICollection<Tag> GetAllTagsForNew(Guid newId);
    }
}
