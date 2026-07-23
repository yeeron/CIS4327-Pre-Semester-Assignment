using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models;

// A skill a volunteer can have. Many-to-many with Volunteer via VolunteerSkill.
public class Skill
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<VolunteerSkill> VolunteerSkills { get; set; } = new List<VolunteerSkill>();
}
