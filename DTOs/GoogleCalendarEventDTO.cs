using GoogleCalendarAPI.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace GoogleCalendarAPI.DTOs
{
    public class GoogleCalendarEventDTO
    {
        [Required]
        public string? Summary { get; set; }

        public string Description { get; set; } = string.Empty;

        [Required]

        [ValidateFutureDateTime]
        [ValidateCreationDays]
        public DateTime? StartTime { get; set; }

        [Required]

        [ValidateFutureDateTime]
        [ValidateCreationDays]
        public DateTime? EndTime { get; set; }
    }
}
