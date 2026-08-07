namespace VolunteerMS.Models.ViewModels.Skill;
public class SkillIndexVM
{
    public string? SearchTerm { get; set; }
    public List<SkillListVM> Skills { get; set; } = new();
}