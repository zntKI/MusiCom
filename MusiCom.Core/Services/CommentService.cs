using MusiCom.Core.Contracts;
using MusiCom.Core.Models.Comment;
using MusiCom.Infrastructure.Data.Common;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository repo;

        public CommentService(IRepository _repo)
        {
            repo = _repo;
        }

        public async Task AddDislikeToCommentAsync(NewComment comment)
        {
            comment.NumberOfDislikes++;

            await repo.SaveChangesAsync();
        }

        public async Task AddLikeToCommentAsync(NewComment comment)
        {
            comment.NumberOfLikes++;

            await repo.SaveChangesAsync();
        }

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

        public async Task<NewComment> GetCommentByIdAsync(Guid commentId)
        {
            return await repo.GetByIdAsync<NewComment>(commentId);
        }
    }
}
