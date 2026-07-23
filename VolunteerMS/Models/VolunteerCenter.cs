namespace VolunteerMS.Models;

// Explicit join: a volunteer is affiliated with a center.
// Composite primary key (VolunteerId, CenterId) configured in AppDbContext.
public class VolunteerCenter
{
    public int VolunteerId { get; set; }
    public Volunteer Volunteer { get; set; } = null!;

    public int CenterId { get; set; }
    public Center Center { get; set; } = null!;
}
