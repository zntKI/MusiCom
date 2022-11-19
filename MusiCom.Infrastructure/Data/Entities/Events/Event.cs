using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusiCom.Infrastructure.Data.DataConstraints.EventC;

namespace MusiCom.Infrastructure.Data.Entities.Events
{
    /// <summary>
    /// Contains data for somekind of Musical Event
    /// </summary>
    public class Event
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(EventTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        /// <summary>
        /// The Artist who have posted the Event
        /// </summary>
        [Required]
        [ForeignKey(nameof(Artist))]
        public Guid ArtistId { get; set; }

        public ApplicationUser Artist { get; set; } = null!;

        /// <summary>
        /// Posts related to the Event
        /// </summary>
        public ICollection<EventPost> EventPosts { get; set; } = new List<EventPost>();

        [Required]
        public DateTime DateOfCreation { get; init; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
