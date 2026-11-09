namespace RaceManagementMVC.Models
{
    public class loginResponse
    {
        public string token { get; set; } = string.Empty;

        public userResponse user { get; set; } = new userResponse();
    }
}
