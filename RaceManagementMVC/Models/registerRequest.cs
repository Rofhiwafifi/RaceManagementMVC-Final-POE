using System.ComponentModel.DataAnnotations;

namespace RaceManagementMVC.Models
{
    public class registerRequest
    {
        [Required]
        public string firstName { get; set; } = string.Empty;

        [Required]
        public string lastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        public string password { get; set; } = string.Empty;

        [Required]
        public string role { get; set; } = string.Empty;
    }
}
