using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MusiCom.Core.Models.Genre;
using MusiCom.Core.Models.Tag;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusiCom.Core.Models.New
{
    /// <summary>
    /// Contains New creation data
    /// </summary>
    //TODO: Fix
    public class AddNewViewModel
    {
        [Required]
        [MaxLength()]
        public string Title { get; set; } = null!;

        [Required]
        public byte[] TitlePhoto { get; set; } = null!;

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Content { get; set; } = null!;

        [Required]
        public Guid GenreId { get; set; }
        public IEnumerable<GenreViewModel> Genres { get; set; } = new List<GenreViewModel>();

        [Required]
        public Guid EditorId { get; set; }

        public IEnumerable<TagViewModel> Tags { get; set; } = new List<TagViewModel>();
    }
}
