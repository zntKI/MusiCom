using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.NewCommentC;

namespace MusiCom.Core.Models.Comment
{
    /// <summary>
    /// Contains data for adding a Comment
    /// </summary>
    public class CommentAddViewModel
    {
        [Required]
        [MinLength(ContentMinLength, ErrorMessage = "Content must be at least {1} symbols")]
        [MaxLength(ContentMaxLength, ErrorMessage = "Content must be up to {1} symbols")]
        public string Content { get; set; } = null!;
    }
}
