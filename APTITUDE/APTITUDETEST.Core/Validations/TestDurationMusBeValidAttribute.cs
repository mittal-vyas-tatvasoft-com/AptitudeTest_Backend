using System.ComponentModel.DataAnnotations;


namespace AptitudeTest.Core.Validations
{
    internal class TestDurationMusBeValidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var testDuration = (int)value;
            var startTimeProperty = validationContext.ObjectType.GetProperty("StartTime");
            var endTimeProperty = validationContext.ObjectType.GetProperty("EndTime");
            var startTime = (DateTime)startTimeProperty.GetValue(validationContext.ObjectInstance);
            var endTime = (DateTime)endTimeProperty.GetValue(validationContext.ObjectInstance);

            TimeSpan duration = (DateTime)endTime - (DateTime)startTime;

            if (duration.Minutes != testDuration)             
            {
                return new ValidationResult("Duration must be valid");
            }

            return ValidationResult.Success;
        }
    }
}