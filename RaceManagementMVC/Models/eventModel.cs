namespace RaceManagementMVC.Models
{
    public class eventModel
    {
        public int eventId { get; set; }

        public int organiserId { get; set; }

        public string eventName { get; set; } = string.Empty;

        public string description { get; set; } = string.Empty;

        public DateTime eventDate { get; set; }

        public TimeSpan startTime { get; set; }

        public string venue { get; set; } = string.Empty;

        public decimal distance { get; set; }

        public string eventType { get; set; } = string.Empty;

        public int maxParticipants { get; set; }

        public string status { get; set; } = string.Empty;

        public string? bannerImageUrl { get; set; }
    }
}
