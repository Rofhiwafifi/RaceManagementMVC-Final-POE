namespace RaceManagementMVC.Models
{
    public class enrolmentModel
    {
        public int enrolmentId { get; set; }

        public int eventId { get; set; }

        public int participantId { get; set; }

        public int categoryId { get; set; }

        public DateTime enrolmentDate { get; set; }

        public string status { get; set; } = string.Empty;

        public string eventName { get; set; } = string.Empty;

        public string participantName { get; set; } = string.Empty;

        public string categoryName { get; set; } = string.Empty;
    }
}