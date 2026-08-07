using VolunteerMS.Utilities;

namespace VolunteerMS.Models.ViewModels.Volunteer;
public class VolunteerIndexVM
{
    public string? SearchTerm { get; set; }
    public VolunteerFilter Filter { get; set; } = VolunteerFilter.Default;
    public IEnumerable<VolunteerListVM> Volunteers { get; set; } = new List<VolunteerListVM>();
}