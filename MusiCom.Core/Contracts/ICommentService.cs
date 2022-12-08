using MusiCom.Core.Models.Comment;
using MusiCom.Infrastructure.Data.Entities.News;

namespace MusiCom.Core.Contracts
{
    public interface ICommentService
    {
        /// <summary>
        /// Creates a Comment atached to a given New
        /// </summary>
        /// <param name="model">ViewModel containing data for the Comment's Content</param>
        /// <param name="newId">New's Id</param>
        /// <param name="userId">User's Id</param>
        Task CreateCommentAsync(CommentAddViewModel model, Guid newId, Guid userId);

        /// <summary>
        /// Adds Like to Comment
        /// </summary>
        /// <param name="comment">Comment to which a like to be added</param>
        Task AddLikeToCommentAsync(NewComment comment);

        /// <summary>
        /// Adds Dislike to Comment
        /// </summary>
        /// <param name="comment">Comment to which a dislike to be added</param>
        Task AddDislikeToCommentAsync(NewComment comment);

        /// <summary>
        /// Gets the Comment corresponding to the given Id
        /// </summary>
        /// <param name="commentId">Id of the Comment</param>
        Task<NewComment> GetCommentByIdAsync(Guid commentId);
    }
}
