namespace VolunteerMS.Models;

// Explicit join: a volunteer is matched to an opportunity (the "match" record).
// Composite primary key (VolunteerId, OpportunityId) configured in AppDbContext.
// Carries DateMatched so the "View Matches" screens can show when the match was made.
// Currently created manually by the admin; auto-matching may be added later.

public class VolunteerOpportunity
{
    public int VolunteerId { get; set; }
    public Volunteer Volunteer { get; set; } = null!;

    public int OpportunityId { get; set; }
    public Opportunity Opportunity { get; set; } = null!;

    public DateTime DateMatched { get; set; } = DateTime.UtcNow;
}
