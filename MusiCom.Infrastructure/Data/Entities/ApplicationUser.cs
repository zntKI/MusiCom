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
    }
}
