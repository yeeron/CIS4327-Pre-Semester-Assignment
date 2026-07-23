using Microsoft.EntityFrameworkCore;
using VolunteerMS.Models;

namespace VolunteerMS.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Volunteer> Volunteers => Set<Volunteer>();
    public DbSet<Opportunity> Opportunities => Set<Opportunity>();
    public DbSet<Center> Centers => Set<Center>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<VolunteerCenter> VolunteerCenters => Set<VolunteerCenter>();
    public DbSet<VolunteerSkill> VolunteerSkills => Set<VolunteerSkill>();
    public DbSet<VolunteerOpportunity> VolunteerOpportunities => Set<VolunteerOpportunity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Username must be unique across users.
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // Volunteer 1:1 User, enforced by a unique index on the FK.
        // If a User is deleted, its Volunteer profile goes too.
        modelBuilder.Entity<Volunteer>()
            .HasOne(v => v.User)
            .WithOne(u => u.Volunteer)
            .HasForeignKey<Volunteer>(v => v.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Volunteer>()
            .HasIndex(v => v.UserId)
            .IsUnique();

        // --- Composite keys on the explicit join entities ---
        modelBuilder.Entity<VolunteerCenter>()
            .HasKey(vc => new { vc.VolunteerId, vc.CenterId });

        modelBuilder.Entity<VolunteerSkill>()
            .HasKey(vs => new { vs.VolunteerId, vs.SkillId });

        modelBuilder.Entity<VolunteerOpportunity>()
            .HasKey(vo => new { vo.VolunteerId, vo.OpportunityId });

        // Deleting an Opportunity cascades to its match rows (hard delete of the opportunity).
        modelBuilder.Entity<VolunteerOpportunity>()
            .HasOne(vo => vo.Opportunity)
            .WithMany(o => o.VolunteerOpportunities)
            .HasForeignKey(vo => vo.OpportunityId)
            .OnDelete(DeleteBehavior.Cascade);

        // Deleting a Volunteer cascades to its join rows.
        modelBuilder.Entity<VolunteerOpportunity>()
            .HasOne(vo => vo.Volunteer)
            .WithMany(v => v.VolunteerOpportunities)
            .HasForeignKey(vo => vo.VolunteerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
