using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusiCom.Infrastructure.Data.DataConstraints.NewC;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    public class New
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(NewTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        public byte[] TitlePhoto { get; set; } = null!;

        [Required]
        public DateTime PostedOn { get; init; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Genre))]
        public Guid GenreId { get; set; }

        public Genre Genre { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Editor))]
        public Guid EditorId { get; set; }

        public ApplicationUser Editor { get; set; } = null!;

        public ICollection<NewTags> Tags { get; set; } = new List<NewTags>();

        public ICollection<NewComment> NewComments { get; set; } = new List<NewComment>();

        [Required]
        public bool IsDeleted { get; set; }
    }
}
