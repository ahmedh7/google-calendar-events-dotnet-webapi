using System;
using System.ComponentModel.DataAnnotations;

namespace GoogleCalendarAPI.ValidationAttributes
{
    public class ValidateCreationDays : ValidationAttribute
    {
        private static readonly string[] RestrictedDays = { "saturday", "friday" };
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            DateTime _dateTime = Convert.ToDateTime(value);
            var dayOfWeek = _dateTime.DayOfWeek.ToString().ToLower();
            Console.WriteLine(dayOfWeek);
            Console.WriteLine(Array.Exists(RestrictedDays, d => d == dayOfWeek));
            if (Array.Exists(RestrictedDays, d => d == dayOfWeek))
            {
                return new ValidationResult("Events can't be created on Saturday or Friday.");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
