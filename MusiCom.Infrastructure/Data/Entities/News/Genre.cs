using MusiCom.Infrastructure.Data.Entities.Events;
using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.GenreC;

namespace MusiCom.Infrastructure.Data.Entities.News
{
    public class Genre
    {
        [Key]
        public Guid Id { get; init; }

        [Required]
        [MaxLength(GenreNameMaxLength)]
        public string Name { get; set; } = null!;

        public ICollection<New> News { get; set; } = new List<New>();

        [Required]
        public DateTime DateOfCreation { get; init; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
