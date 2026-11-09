using System.ComponentModel.DataAnnotations;

namespace RaceManagementMVC.Models
{
    public class updateEventRequest
    {
        [Required]
        public string eventName { get; set; } = string.Empty;

        [Required]
        public string description { get; set; } = string.Empty;

        [Required]
        public DateTime eventDate { get; set; }

        [Required]
        public TimeSpan startTime { get; set; }

        [Required]
        public string venue { get; set; } = string.Empty;

        [Required]
        [Range(0.1, 500)]
        public decimal distance { get; set; }

        [Required]
        public string eventType { get; set; } = string.Empty;

        [Required]
        [Range(1, 100000)]
        public int maxParticipants { get; set; }

        public string status { get; set; } = "Upcoming";

        public List<int> categoryIds { get; set; } = new List<int>();
    }
}
