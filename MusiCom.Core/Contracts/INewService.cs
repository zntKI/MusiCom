using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.Event;
using MusiCom.Core.Models.New;
using MusiCom.Infrastructure.Data.Entities.News;
using static MusiCom.Infrastructure.Data.DataConstraints;
using static System.Net.Mime.MediaTypeNames;

namespace MusiCom.Core.Contracts
{
    /// <summary>
    /// Interface for GenreService
    /// </summary>
    public interface INewServices
    {
        Task<NewQueryServiceModel> GetAllNewsAsync(
            string? genre = null,
            string? tag = null,
            string? searchTerm = null,
            int currentPage = 1,
            int newsPerPage = 1);

        Task CreateNewAsync(Guid userId, NewAddViewModel model, IFormFile image);

        IEnumerable<NewAllNewViewModel> GetLastThreeNews();

        IEnumerable<NewAllNewViewModel> GetRemainingNews();

        Task<New> GetNewByIdAsync(Guid newId);

        ICollection<Tag> GetAllTagsForNew(Guid newId);

        ICollection<NewComment> GetAllCommentsForNew(Guid newId);
        Task DeleteNewAsync(New neww);

        Task EditNewAsync(New neww, NewEditViewModel model, IFormFile image);
    }
}
