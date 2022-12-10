using Microsoft.AspNetCore.Http;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Post;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.Events;

namespace MusiCom.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository repo;

        public PostService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task AddDislikeToPostAsync(EventPost post)
        {
            post.NumberOfDislikes++;

            await repo.SaveChangesAsync();
        }

        public async Task AddLikeToPostAsync(EventPost post)
        {
            post.NumberOfLikes++;

            await repo.SaveChangesAsync();
        }

        public async Task CreatePostAsync(PostAddViewModel model, Guid eventId, Guid userId, IFormFile image)
        {
            EventPost post = new EventPost()
            {
                Content = model.Content,
                DateOfPost = DateTime.Now,
                NumberOfLikes = 0,
                NumberOfDislikes = 0,
                EventId = eventId,
                UserId = userId,
                IsDeleted = false,
            };

            if (image != null)
            {
                await AddImage(post, image);
            }

            await repo.AddAsync(post);
            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Adds the given Image to the Post
        /// </summary>
        /// <param name="post">The Post</param>
        /// <param name="image">The Image</param>
        /// <returns>The modified Post</returns>
        public async Task<EventPost> AddImage(EventPost post, IFormFile image)
        {
            string type = image.ContentType;

            if (!type.Contains("image"))
            {
                throw new InvalidOperationException("Not an image");
            }

            string contentType = type.Substring(type.IndexOf('/') + 1, type.Length - type.Substring(0, type.IndexOf('/')).Length - 1);

            if (contentType != "png" && contentType != "jpeg" && contentType != "jpg")
            {
                throw new InvalidOperationException("Not the right image format");
            }

            if (image.Length > 0)
            {
                using var stream = new MemoryStream();
                await image.CopyToAsync(stream);
                post.Image = stream.ToArray();
            }
            else
            {
                throw new InvalidOperationException("Image else");
            }

            return post;
        }

        public async Task<EventPost> GetPostByIdAsync(Guid postId)
        {
            return await repo.GetByIdAsync<EventPost>(postId);
        }
    }
}
