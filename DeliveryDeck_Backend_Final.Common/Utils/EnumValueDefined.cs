using System.ComponentModel.DataAnnotations;

namespace DeliveryDeck_Backend_Final.Common.Utils
{
    public class EnumValueDefined : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            Type enumType = value.GetType();
            return Enum.IsDefined(enumType, value) ?
                ValidationResult.Success :
                new ValidationResult(string.Format("{0} is not a valid value for a type {1}", value, enumType));

        }
    }
}
