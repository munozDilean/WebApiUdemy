
using System.ComponentModel.DataAnnotations;

namespace WebAPIUdemy.Models.Validations{
    public class Shirt_EnsureCorrectSizingAttribute : ValidationAttribute {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var shirt = validationContext.ObjectInstance as Shirt;
            if (shirt != null && !string.IsNullOrWhiteSpace(shirt.Gender)) {
                if (shirt.Gender.Equals("men", StringComparison.OrdinalIgnoreCase) && shirt.Size < 8){
                    return new ValidationResult("For men, size need ot be bigger than 8 or 8");
                }
                else if (shirt.Gender.Equals("women", StringComparison.OrdinalIgnoreCase) && shirt.Size < 6){
                    return new ValidationResult("For women, size need ot be bigger than 6 or 6");
                }
            }
            return ValidationResult.Success;
        }
    }
}