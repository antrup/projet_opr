using BugTrackerAPI.Repositories;
using System.ComponentModel.DataAnnotations;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace BugTrackerAPI.Data.Validation
{
    [AttributeUsage(AttributeTargets.Property |
 AttributeTargets.Field, AllowMultiple = false)]
    public class ApplicationIDValidation : ValidationAttribute
    {
        // Custom validation rule to check if applicationID provided with a ticket creation request exists
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var service = (IApplicationRepository)validationContext
                         .GetService(typeof(IApplicationRepository))!;
            if (value != null)
            {
                var ApplicationID = (int)value;
                if (service.ApplicationExists(ApplicationID))
                    return ValidationResult.Success!;
                return new ValidationResult("Application ID is invalid");

            }
            return new ValidationResult("Application ID is missing");
        }
    }
}
