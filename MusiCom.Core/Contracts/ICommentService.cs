﻿using MusiCom.Core.Models.Comment;

namespace MusiCom.Core.Contracts
{
    public interface ICommentService
    {
        Task CreateCommentAsync(CommentAddViewModel model, Guid newId, Guid userId);

        Task AddLikeToComment(Guid commentId);

        Task AddDislikeToComment(Guid commentId);
    }
}
