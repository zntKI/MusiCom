using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.TagC;

namespace MusiCom.Core.Models.Tag
{
    /// <summary>
    /// Contains Tag data
    /// </summary>
    public class TagViewModel
    {
        [Required]
        [StringLength(TagNameMaxLength, MinimumLength = TagNameMinLength, ErrorMessage = "Tag name must be between {2} and {1} symbols")]
        public string Name { get; set; } = null!;
    }
}
