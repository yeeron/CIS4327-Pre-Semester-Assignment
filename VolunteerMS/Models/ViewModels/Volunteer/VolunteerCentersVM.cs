using VolunteerMS.Models.ViewModels.Center;

namespace VolunteerMS.Models.ViewModels.Volunteer;
public class VolunteerCentersVM
{
    public int VolunteerId { get; set; }

    public string VolunteerName { get; set; } = string.Empty;

    public List<CenterSelectionVM> Centers { get; set; } = new();
}