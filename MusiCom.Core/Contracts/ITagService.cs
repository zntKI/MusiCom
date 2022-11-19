using MusiCom.Core.Models.Tag;

namespace MusiCom.Core.Contracts
{
    /// <summary>
    /// Interface for TagService
    /// </summary>
    public interface ITagService
    {
        Task CreateTagAsync(TagViewModel model);
    }
}
