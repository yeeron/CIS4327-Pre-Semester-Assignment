namespace VolunteerMS.Models;

// Explicit join: a volunteer has a skill.
// Composite primary key (VolunteerId, SkillId) configured in AppDbContext.
public class VolunteerSkill
{
    public int VolunteerId { get; set; }
    public Volunteer Volunteer { get; set; } = null!;

    public int SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
}
