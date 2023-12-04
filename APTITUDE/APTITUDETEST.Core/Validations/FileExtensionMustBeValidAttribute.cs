using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.Validations
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FileExtensionMustBeValidAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions;

        public FileExtensionMustBeValidAttribute(params string[] allowedExtensions)
        {
            _allowedExtensions = allowedExtensions;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName);
                if (!_allowedExtensions.Any(ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)))
                {
                    return new ValidationResult($"File extension not allowed. Only {string.Join(", ", _allowedExtensions)} extensions are allowed.");
                }
            }

            return ValidationResult.Success!;
        }
    }
}
