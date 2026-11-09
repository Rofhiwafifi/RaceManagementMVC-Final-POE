using System.ComponentModel.DataAnnotations;

namespace RaceManagementMVC.Models
{
    public class createCategoryRequest
    {
        [Required]
        public string categoryName { get; set; } = string.Empty;

        [Required]
        public string description { get; set; } = string.Empty;
    }
}