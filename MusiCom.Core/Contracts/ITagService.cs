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
        /// <summary>
        /// Creates a Tag
        /// </summary>
        /// <param name="model">model passed by the controller</param>
        Task CreateTagAsync(TagViewModel model);

        /// <summary>
        /// Lists all Tags ordered by DateOfCreation
        /// </summary>
        /// <returns>All Tags</returns>
        Task<IEnumerable<TagNewAllViewModel>> GetAllTagsAsync();

        /// <summary>
        /// Gets the Tag with the given Id
        /// </summary>
        /// <param name="Id">Id of a Tag</param>
        /// <returns>The desired Tag</returns>
        Task<Tag> GetTagByIdAsync(Guid Id);

        /// <summary>
        /// Edits a Tag with a given Id
        /// </summary>
        /// <param name="tag">Tag</param>
        /// <param name="model">Model which is passed from the View</param>
        Task EditTagAsync(Tag tag, TagAllViewModel model);

        /// <summary>
        /// Marks a given Tag as Deleted
        /// </summary>
        /// <param name="tag">Tag</param>
        Task DeleteTagAsync(Tag tag);

        /// <summary>
        /// Gets the Tags' Names
        /// </summary>
        Task<IEnumerable<string>> GetAllTagNamesAsync();
    }
}
