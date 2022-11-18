using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.TagC;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    public class Tag
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(TagNameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<NewTags> News { get; set; } = new List<NewTags>();

        [Required]
        public DateTime DateOfCreation { get; init; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
