using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.EventPostC;

namespace MusiCom.Core.Models.Post
{
    /// <summary>
    /// Contains data for adding a Post
    /// </summary>
    public class PostAddViewModel
    {
        [Required]
        [MinLength(ContentMinLength, ErrorMessage = "Content must be at least {1} symbols")]
        [MaxLength(ContentMaxLength, ErrorMessage = "Content must be up to {1} symbols")]
        public string Content { get; set; } = null!;

        public byte[]? Image { get; set; }
    }
}
