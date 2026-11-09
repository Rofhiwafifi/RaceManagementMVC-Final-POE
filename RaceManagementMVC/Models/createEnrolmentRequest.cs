using System.ComponentModel.DataAnnotations;

namespace RaceManagementMVC.Models
{
    public class createEnrolmentRequest
    {
        [Required]
        public int categoryId { get; set; }
    }
}
