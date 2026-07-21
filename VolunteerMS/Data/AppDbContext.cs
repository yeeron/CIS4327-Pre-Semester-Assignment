using Microsoft.EntityFrameworkCore;
using VolunteerMS.Models;

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

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();
    }
}
