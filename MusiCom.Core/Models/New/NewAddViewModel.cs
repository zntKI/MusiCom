using Microsoft.AspNetCore.Mvc.Rendering;
using MusiCom.Core.Models.CustomAttributes;
using MusiCom.Core.Models.Genre;
using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.NewC;

namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Contains New creation data
    /// </summary>
    public class NewAddViewModel
    {
        [Required]
        [StringLength(NewTitleMaxLength, MinimumLength = NewTitleMinLength, ErrorMessage = "The name field must be between {2} and {1} symbols long.")]
        public string Title { get; set; } = null!;

        [Required]
        //[ValidateImageSize]
        //[RegularExpression(pattern: "([^\\s]+(\\.(?i)(jpg|jpeg|png))$)", ErrorMessage = "Please insert an image file.")]
        public byte[] TitlePhoto { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public Guid GenreId { get; set; }
        public IEnumerable<GenreAllViewModel> Genres { get; set; } = new List<GenreAllViewModel>();

        [Required]
        public Guid EditorId { get; set; }

        public IEnumerable<Guid> Tags { get; set; } = new List<Guid>();

        public IEnumerable<SelectListItem> TagsAll { get; set; } = new List<SelectListItem>();
    }
}
