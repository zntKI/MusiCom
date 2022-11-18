using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusiCom.Infrastructure.Data.DataConstraints.NewCommentC;

namespace MusiCom.Infrastructure.Data.Entities.News
{
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

        [Required]
        [ForeignKey(nameof(New))]
        public Guid NewId { get; init; }

        public New New { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(User))]
        public Guid UserId { get; init; }

        public ApplicationUser User { get; set; } = null!;

        [Required]
        public bool IsDeleted { get; set; }
    }
}
