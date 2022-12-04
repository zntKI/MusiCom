using MusiCom.Core.Models.Genre;
using MusiCom.Core.Models.Tag;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Contracts
{
    /// <summary>
    /// Interface for TagService
    /// </summary>
    public interface ITagService
    {
        Task CreateTagAsync(TagViewModel model);

        Task<IEnumerable<TagNewAllViewModel>> GetAllTags();

        Task<Tag> GetTagByIdAsync(Guid Id);

        Task EditTagAsync(Guid id, TagAllViewModel model);

        Task DeleteTagAsync(Guid id);
    }
}
