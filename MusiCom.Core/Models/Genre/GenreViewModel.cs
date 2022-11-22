using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.GenreC;

namespace MusiCom.Core.Models.Genre
{
    /// <summary>
    /// Contains Genre data for Adding
    /// </summary>
    public class GenreViewModel
    {
        [Required]
        [StringLength(GenreNameMaxLength, MinimumLength = GenreNameMinLength, ErrorMessage = "Genre name must be between {2} and {1} symbols long")]
        public string Name { get; set; } = null!;
    }
}
