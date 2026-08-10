namespace VolunteerMS.Models.ViewModels.Opportunity;
public class OpportunitySelectionVM
{
    public int OpportunityId { get; set; }
    public string OpportunityName { get; set; } = string.Empty;
    public string CenterName { get; set; } = string.Empty;
    public bool IsActive {get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool Selected { get; set; }
}