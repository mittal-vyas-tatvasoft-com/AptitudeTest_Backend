using System.ComponentModel.DataAnnotations;


namespace AptitudeTest.Core.Validations
{
    public class DateMustBeValidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var dateProperty = validationContext.ObjectType.GetProperty("Date");
            DateTime? date;
            if (dateProperty != null)
            {
                date = (DateTime?)dateProperty.GetValue(validationContext.ObjectInstance);
                if (date < DateTime.Today)
                {
                    return new ValidationResult("Date must be valid");
                }
            }
            return ValidationResult.Success;
        }
    }
}
