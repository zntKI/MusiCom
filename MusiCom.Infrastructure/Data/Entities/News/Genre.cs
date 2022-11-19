using MusiCom.Infrastructure.Data.Entities.Events;
using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.GenreC;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    /// <summary>
    /// Contains data for News' Musical Genres
    /// </summary>
    public class Genre
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// The News which are from the given Genre
        /// </summary>
        public ICollection<New> News { get; set; } = new List<New>();

        [Required]
        public DateTime DateOfCreation { get; init; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
