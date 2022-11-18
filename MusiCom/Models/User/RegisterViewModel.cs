using System.ComponentModel.DataAnnotations;
using static MusiCom.Infrastructure.Data.DataConstraints.UserC;

namespace MusiCom.Models.User
{
    /// <summary>
    /// Register data
    /// </summary>
    /// <remarks>
    /// Contains information about the data which is required for a user to register.
    /// </remarks>
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "First name field is required.")]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength, ErrorMessage = "First name must be between {2} and {1} symbols long.")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name field is required.")]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength, ErrorMessage = "Last name must be between {2} and {1} symbols long.")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Username field is required.")]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength, ErrorMessage = "Username must be between {2} and {1} symbols long.")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Email field is required.")]
        [EmailAddress]
        [StringLength(UserEmailMaxLength, MinimumLength = UserEmailMinLength, ErrorMessage = "Email must be between {2} and {1} symbols long.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password field is required.")]
        [StringLength(UserPasswordMaxLength, MinimumLength = UserPasswordMinLength, ErrorMessage = "Password must be between {2} and {1} symbols long.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
