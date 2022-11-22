using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MusiCom.Core.Models.CustomAttributes
{
    /// <summary>
    /// Validates the size of the Image
    /// </summary>
    public class ValidateImageSizeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file?.Length < 1 * 1024 * 1024)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage = "Add a smaller Image");
        }
    }
}
