using System.ComponentModel.DataAnnotations;


namespace AptitudeTest.Core.Validations
{
    internal class TestDurationMusBeValidAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int testDuration = 0;
            if (value != null)
            {
                testDuration = (int)value;
            }
            var startTimeProperty = validationContext.ObjectType.GetProperty("StartTime");
            var endTimeProperty = validationContext.ObjectType.GetProperty("EndTime");
            if (startTimeProperty != null && endTimeProperty != null)
            {
                var startTime = (DateTime)startTimeProperty.GetValue(validationContext.ObjectInstance);
                var endTime = (DateTime)endTimeProperty.GetValue(validationContext.ObjectInstance);

                TimeSpan duration = endTime - startTime;

                if (duration.TotalMinutes != testDuration)
                {
                    return new ValidationResult("Duration must be valid");
                }
            }
            return ValidationResult.Success;
        }
    }
}