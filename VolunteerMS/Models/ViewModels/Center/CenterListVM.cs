namespace VolunteerMS.Models.ViewModels.Center;

public class CenterListVM
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int VolunteerCount { get; set; }
    public int OpportunityCount { get; set; }
}