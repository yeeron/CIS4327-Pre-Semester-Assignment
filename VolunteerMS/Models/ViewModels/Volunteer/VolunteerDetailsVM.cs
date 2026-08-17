namespace VolunteerMS.Models.ViewModels.Volunteer;
public class VolunteerDetailsVM : VolunteerBaseVM
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public ApprovalStatus ApprovalStatus { get; set; }
    public DateTime RegistrationDate { get; set; }
    public List<string> Skills { get; set; } = new();
    public List<string> Centers { get; set; } = new();
    public bool IsActive{get; set;}
    public List<string> Opportunities { get; set; } = new();
}