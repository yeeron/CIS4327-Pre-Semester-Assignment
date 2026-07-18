using Microsoft.EntityFrameworkCore;

namespace VolunteerMS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Entities land here during Foundation (week 1). Planned, per the project plan:
    //   AdminUser, Volunteer, Opportunity, Center, Skill,
    //   VolunteerCenter, VolunteerSkill, VolunteerOpportunity
    //
    // public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    // ...
}
