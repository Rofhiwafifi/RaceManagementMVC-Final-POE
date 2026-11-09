using System.ComponentModel.DataAnnotations;

namespace RaceManagementMVC.Models
{
    public class createResultRequest
    {
        [Required]
        public int enrolmentId { get; set; }

        [Required]
        [Range(1, 100000)]
        public int position { get; set; }

        [Required]
        public TimeSpan finishTime { get; set; }

        public decimal score { get; set; }

        public string resultStatus { get; set; } = "Published";
    }
}
