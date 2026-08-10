using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models.ViewModels.Skill;
public class SkillBaseVM
{
    [Required]
    [Display(Name = "Skill Name")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}