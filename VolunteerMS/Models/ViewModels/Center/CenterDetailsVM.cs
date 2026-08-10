namespace VolunteerMS.Models.ViewModels.Center;
public class CenterDetailsVM : CenterBaseVM
{
    public int Id { get; set; }
    public int VolunteerCount { get; set; }
    public int OpportunityCount { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<Models.Opportunity> Opportunities { get; set; } = new();
}