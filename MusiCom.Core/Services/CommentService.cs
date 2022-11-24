using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Comment;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities;
using MusiCom.Infrastructure.Data.Entities.News;
using static MusiCom.Infrastructure.Data.DataConstraints;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MusiCom.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository repo;

        public CommentService(IRepository _repo)
        {
            repo = _repo;
        }

        /// <summary>
        /// Adds Dislike to Comment
        /// </summary>
        /// <param name="commentId">Comment Id</param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task AddDislikeToComment(Guid commentId)
        {
            var comment = await repo.GetByIdAsync<NewComment>(commentId);

            if (comment == null)
            {
                throw new InvalidOperationException();
            }

            comment.NumberOfDislikes++;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Adds Like to Comment
        /// </summary>
        /// <param name="commentId">Comment Id</param>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task AddLikeToComment(Guid commentId)
        {
            var comment = await repo.GetByIdAsync<NewComment>(commentId);

            if (comment == null)
            {
                throw new InvalidOperationException();
            }

            comment.NumberOfLikes++;

            await repo.SaveChangesAsync();
        }

        /// <summary>
        /// Creates a Comment atached to a given New
        /// </summary>
        /// <param name="model">ViewModel containing data for the Comment's Content</param>
        /// <param name="newId">New Id</param>
        /// <param name="userId">User Id</param>
        public async Task CreateCommentAsync(CommentAddViewModel model, Guid newId, Guid userId)
        {
            NewComment comment = new NewComment()
            {
                Content = model.Content,
                DateOfPost = DateTime.Now,
                NumberOfLikes = 0,
                NumberOfDislikes = 0,
                NewId = newId,
                UserId = userId,
                IsDeleted = false
            };

            await repo.AddAsync(comment);
            await repo.SaveChangesAsync();
        }
    }
}
