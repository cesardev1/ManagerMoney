
using System.ComponentModel.DataAnnotations;

namespace ManagerMoney.Validations
{
    public class FirstLetterUpperAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;

            var FirstLetter = value.ToString()[0].ToString();

            if (FirstLetter != FirstLetter.ToUpper())
                return new ValidationResult("La primera letra debe ser mayuscula");
            return ValidationResult.Success;

        }
    }
}
