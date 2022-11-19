using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusiCom.Infrastructure.Data.DataConstraints.EventPostC;

namespace MusiCom.Infrastructure.Data.Entities.Events
{
    /// <summary>
    /// Contains Data for User Posts related to a given Event
    /// </summary>
    public class EventPost
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(ContentMaxLength)]
        public string Content { get; set; } = null!;

        public byte[]? Photo { get; set; }

        [Required]
        public DateTime DateOfPost { get; init; }

        public DateTime? DateOfChange { get; set; }

        [Required]
        [ForeignKey(nameof(Event))]
        public Guid EventId { get; set; }

        public Event Event { get; set; } = null!;

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
