using Microsoft.AspNetCore.Mvc.Rendering;
using OpportunityModel = VolunteerMS.Models.Opportunity;

namespace VolunteerMS.Models.ViewModels.Opportunity;
public class OpportunityIndexVM
{
    public string? SearchTerm { get; set; }
    public bool RecentOnly { get; set; }
    public int? CenterId { get; set; }
    public IEnumerable<OpportunityModel> Opportunities { get; set; } = new List<OpportunityModel>();
    public IEnumerable<SelectListItem> Centers { get; set; } = new List<SelectListItem>();
}