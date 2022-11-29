using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.Post;

namespace MusiCom.Core.Contracts
{
    public interface IPostService
    {
        Task CreatePostAsync(PostAddViewModel model, Guid eventId, Guid userId, IFormFile image);

        Task AddLikeToPost(Guid postId);

        Task AddDislikeToPost(Guid postId);
    }
}
