namespace RaceManagementMVC.Models
{
    public class OrganiserEventSelectionViewModel
    {
        public int? SelectedEventId { get; set; }
        public List<eventModel> Events { get; set; } = new();
        public List<enrolmentModel> Enrolments { get; set; } = new();
        public List<resultModel> Results { get; set; } = new();
    }
}

