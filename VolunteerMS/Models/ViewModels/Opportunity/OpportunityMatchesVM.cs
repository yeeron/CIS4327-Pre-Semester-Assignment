namespace VolunteerMS.Models.ViewModels.Opportunity;

// Read-only view model for the opportunity-side "matched volunteers" screen.
// Shows which volunteers are matched to a given opportunity (the Manage Opportunities
// use case's "view matches"). Assignment/removal of matches lives on the volunteer side.
public class OpportunityMatchesVM
{
    public int OpportunityId { get; set; }
    public string OpportunityName { get; set; } = string.Empty;
    public string CenterName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public bool IsActive { get; set; }

    public List<MatchedVolunteerVM> Volunteers { get; set; } = new();
}

public class MatchedVolunteerVM
{
    public int VolunteerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ApprovalStatus ApprovalStatus { get; set; }
    public bool IsActive { get; set; }
    public DateTime DateMatched { get; set; }
}
