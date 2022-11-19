using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusiCom.Infrastructure.Data.DataConstraints.NewCommentC;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    /// <summary>
    /// Contains data for the Comments of a given Musical New
    /// </summary>
    public class NewComment
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        [Required]
        public DateTime DateOfPost { get; init; }

        public DateTime? DateOfChange { get; set; }

        [Required]
        public int NumberOfLikes { get; set; }

        [Required]
        public int NumberOfDislikes { get; set; }

        /// <summary>
        /// The New to which it belongs
        /// </summary>
        [Required]
        [ForeignKey(nameof(New))]
        public Guid NewId { get; init; }

        public New New { get; set; } = null!;

        /// <summary>
        /// The User who have posted it
        /// </summary>
        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; init; }

        public ApplicationUser User { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }
    }
}
