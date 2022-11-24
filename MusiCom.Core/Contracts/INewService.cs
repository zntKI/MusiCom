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
        Task CreateNewAsync(Guid userId, NewAddViewModel model, IFormFile image);

        IEnumerable<NewLastThreeViewModel> GetLastThreeNews();

        IEnumerable<NewLastThreeViewModel> GetRemainingNews();

        Task<New> GetNewByIdAsync(Guid newId);

        ICollection<Tag> GetAllTagsForNew(Guid newId);

        ICollection<NewComment> GetAllCommentsForNew(Guid newId);
    }
}
