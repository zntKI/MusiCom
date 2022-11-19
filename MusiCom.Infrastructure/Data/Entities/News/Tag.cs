using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.TagC;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    /// <summary>
    /// Contains data for Tags which are attached to different Musical News
    /// </summary>
    public class Tag
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(TagNameMaxLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The New to which it is attached
        /// </summary>
        public ICollection<NewTags> News { get; set; } = new List<NewTags>();

        [Required]
        public DateTime DateOfCreation { get; init; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
