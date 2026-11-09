using System.ComponentModel.DataAnnotations;

namespace RaceManagementMVC.Models
{
    public class updateProfileRequest
    {
        [Required]
        public string name { get; set; } = string.Empty;

        [Required]
        public string surname { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;
    }
}
