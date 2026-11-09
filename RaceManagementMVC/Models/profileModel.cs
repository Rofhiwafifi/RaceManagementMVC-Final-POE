namespace RaceManagementMVC.Models
{
    public class profileModel
    {
        public int userId { get; set; }
        public string name { get; set; } = string.Empty;
        public string surname { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string? profilePictureUrl { get; set; }
    }
}
