using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.UserC;

namespace MusiCom.Models.User
{
    /// <summary>
    /// Login data
    /// </summary>
    /// <remarks>
    /// Contains information about the data which is required for a user to login.
    /// </remarks>
    public class LoginViewModel
    {
        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        public string UserName { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
