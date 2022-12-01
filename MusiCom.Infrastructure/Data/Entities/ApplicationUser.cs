using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static MusiCom.Infrastructure.Data.DataConstraints.UserC;

namespace MusiCom.Infrastructure.Data.Entities
{
    /// <summary>
    /// Extension data for the IdentityUser class
    /// </summary>
    public class ApplicationUser : IdentityUser<Guid>
    {
        [MaxLength(FirstNameMaxLength)]
        public string? FirstName { get; set; }

        [MaxLength(LastNameMaxLength)]
        public string? LastName { get; set; }

        public byte[]? Image { get; set; }

        [ForeignKey(nameof(Editor))]
        public Guid? EditorId { get; set; }

        public Editor? Editor { get; set; }

        [ForeignKey(nameof(Artist))]
        public Guid? ArtistId { get; set; }

        public Artist? Artist { get; set; }

        public DateTime? DateOfCreation { get; set; }

        [Required]
        public bool IsDeleted { get; set; }
    }
}
