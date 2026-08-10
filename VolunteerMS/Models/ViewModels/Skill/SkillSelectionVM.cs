namespace VolunteerMS.Models.ViewModels.Skill;
public class SkillSelectionVM
{
    public int SkillId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public bool Selected { get; set; }
}