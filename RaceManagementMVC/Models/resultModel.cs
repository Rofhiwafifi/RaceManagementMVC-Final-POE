namespace RaceManagementMVC.Models
{
    public class resultModel
    {
        public int resultId { get; set; }

        public int enrolmentId { get; set; }

        public int position { get; set; }

        public TimeSpan finishTime { get; set; }

        public decimal score { get; set; }

        public string resultStatus { get; set; } = string.Empty;

        public string eventName { get; set; } = string.Empty;

        public DateTime eventDate { get; set; }

        public string categoryName { get; set; } = string.Empty;

        public int totalFinishers { get; set; }
    }
}
