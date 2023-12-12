using System.ComponentModel.DataAnnotations;

namespace GoogleCalendarAPI.ValidationAttributes
{
    public class ValidateFutureDateTime : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            DateTime _dateTime = Convert.ToDateTime(value);
            if (_dateTime > DateTime.Now)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult("Events can't be created in the past.");
            }
        }
    }
}
