using Microsoft.AspNetCore.Http;
using MusiCom.Core.Models.Post;
using MusiCom.Infrastructure.Data.Entities.Events;

namespace MusiCom.Core.Contracts
{
    public interface IPostService
    {
        /// <summary>
        /// Creates a Post atached to a given Event
        /// </summary>
        /// <param name="model">ViewModel containing data for the Post's Content</param>
        /// <param name="eventId">Event Id</param>
        /// <param name="userId">User Id</param>
        /// <param name="image">Post Image</param>
        Task CreatePostAsync(PostAddViewModel model, Guid eventId, Guid userId, IFormFile image);

        /// <summary>
        /// Adds Like to Post
        /// </summary>
        /// <param name="post">Post</param>
        Task AddLikeToPostAsync(EventPost post);

        /// <summary>
        /// Adds Dislike to Post
        /// </summary>
        /// <param name="post">Post</param>
        Task AddDislikeToPostAsync(EventPost post);

        /// <summary>
        /// Gets the Post by the given Id
        /// </summary>
        /// <param name="postId">PostId</param>
        Task<EventPost> GetPostByIdAsync(Guid postId);
    }
}
