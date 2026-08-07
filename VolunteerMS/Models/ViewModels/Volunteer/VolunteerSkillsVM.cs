using VolunteerMS.Models.ViewModels.Skill;

namespace VolunteerMS.Models.ViewModels.Volunteer;
public class VolunteerSkillsVM
{
    public int VolunteerId { get; set; }
    public string VolunteerName { get; set; } = string.Empty;
    public List<SkillSelectionVM> Skills { get; set; } = new();
}