using VolunteerMS.Models.ViewModels.Opportunity;

namespace VolunteerMS.Models.ViewModels.Volunteer;
public class VolunteerOpportunitiesVM
{
    public int VolunteerId { get; set; }
    public string VolunteerName { get; set; } = string.Empty;
    public bool IsActive{get; set;}
    public List<OpportunitySelectionVM> Opportunities { get; set; } = new();
}