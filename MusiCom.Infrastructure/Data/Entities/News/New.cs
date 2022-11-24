using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusiCom.Infrastructure.Data.DataConstraints.NewC;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    /// <summary>
    /// Contains Data for somekind of a Musical New
    /// </summary>
    public class New
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(NewTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Image))]
        public Guid ImageId { get; set; }

        public Image Image { get; set; }

        [Required]
        public DateTime PostedOn { get; init; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = null!;

        /// <summary>
        /// The Musical Genre of the New
        /// </summary>
        [Required]
        [ForeignKey(nameof(Genre))]
        public Guid GenreId { get; set; }

        public Genre Genre { get; set; } = null!;

        /// <summary>
        /// The Editor who have posted the given New
        /// </summary>
        [Required]
        [ForeignKey(nameof(Editor))]
        public Guid EditorId { get; set; }

        public ApplicationUser Editor { get; set; } = null!;

        /// <summary>
        /// Tags which are related to the given New
        /// </summary>
        public ICollection<NewTags> Tags { get; set; } = new List<NewTags>();

        /// <summary>
        /// Comments which are related to given New
        /// </summary>
        public ICollection<NewComment> NewComments { get; set; } = new List<NewComment>();

        [Required]
        public bool IsDeleted { get; set; }
    }
}
