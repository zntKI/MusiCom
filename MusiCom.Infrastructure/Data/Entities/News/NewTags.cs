using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    public class NewTags
    {
        [Required]
        [ForeignKey(nameof(New))]
        public Guid NewId { get; set; }

        public New New { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Tag))]
        public Guid TagId { get; set; }

        public Tag Tag { get; set; } = null!;

        [Required]
        public DateTime DateOfCreation { get; init; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
