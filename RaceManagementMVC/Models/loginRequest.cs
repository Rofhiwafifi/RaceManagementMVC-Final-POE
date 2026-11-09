using System.ComponentModel.DataAnnotations;

namespace RaceManagementMVC.Models
{
    public class loginRequest
    {
        [Required]
        [EmailAddress]
        public string email { get; set; } = string.Empty;

        [Required]
        public string password { get; set; } = string.Empty;
    }
}
