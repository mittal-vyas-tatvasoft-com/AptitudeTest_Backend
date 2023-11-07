

using System.ComponentModel.DataAnnotations;

namespace AptitudeTest.Core.Validations
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EndTimeGreaterThanStartTimeAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var endTimeProperty = validationContext.ObjectType.GetProperty("EndTime");
            var startTime = (DateTime)value;
            var endTime = (DateTime?)endTimeProperty.GetValue(validationContext.ObjectInstance);

            if (endTime <= startTime)
            {
                return new ValidationResult("End Time must be greater than Start Time");
            }

            return ValidationResult.Success;
        }
    }
}
