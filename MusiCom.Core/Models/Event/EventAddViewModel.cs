using MusiCom.Core.Models.Genre;
using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.EventC;

namespace MusiCom.Core.Models.Event
{
    /// <summary>
    /// Contains data for Adding an Event
    /// </summary>
    public class EventAddViewModel
    {
        [Required]
        [MaxLength(EventTitleMaxLength)]
        public string Title { get; set; } = null!;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public byte[] Image { get; set; } = null!;

        [Required]
        public Guid GenreId { get; set; }

        public IEnumerable<GenreAllViewModel> Genres { get; set; } = new List<GenreAllViewModel>();

        /// <summary>
        /// The Artist who have posted the Event
        /// </summary>
        [Required]
        public Guid ArtistId { get; set; }
    }
}
