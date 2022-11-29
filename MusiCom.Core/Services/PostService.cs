using Microsoft.AspNetCore.Http;
using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Comment;
using MusiCom.Core.Models.Post;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.Events;
using MusiCom.Infrastructure.Data.Entities.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MusiCom.Infrastructure.Data.DataConstraints;
using static System.Net.Mime.MediaTypeNames;

namespace MusiCom.Core.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository repo;

        public PostService(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Adds Dislike to Post
        /// </summary>
        /// <param name="postId">Post Id</param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task AddDislikeToPost(Guid postId)
        {
            var post = await repo.GetByIdAsync<EventPost>(postId);

            if (post == null)
            {
                throw new InvalidOperationException();
            }

            post.NumberOfDislikes++;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Adds Like to Post
        /// </summary>
        /// <param name="postId">Post Id</param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task AddLikeToPost(Guid postId)
        {
            var post = await repo.GetByIdAsync<EventPost>(postId);

            if (post == null)
            {
                throw new InvalidOperationException();
            }

            post.NumberOfLikes++;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Creates a Post atached to a given Event
        /// </summary>
        /// <param name="model">ViewModel containing data for the Post's Content</param>
        /// <param name="eventId">Event Id</param>
        /// <param name="userId">User Id</param>
        /// <param name="image">Post Image</param>
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
                string type = image.ContentType;

                if (!type.Contains("image"))
                {
                    throw new InvalidOperationException();
                }

                string contentType = type.Substring(type.IndexOf('/') + 1, type.Length - type.Substring(0, type.IndexOf('/')).Length - 1);

                if (contentType != "png" && contentType != "jpeg" && contentType != "jpg")
                {
                    throw new InvalidOperationException("Please import an image in one of the formats shown above!");
                }

                //TODO: Fix
                if (image.Length > 0)
                {
                    using var stream = new MemoryStream();
                    await image.CopyToAsync(stream);
                    post.Image = stream.ToArray();
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }

            await repo.AddAsync(post);
            await repo.SaveChangesAsync();
        }
    }
}
