using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VolunteerMS.Models;

namespace VolunteerMS.Data;

// Seeds baseline data on startup IF the database is empty. 
// Data is deliberately shaped so every filter in the spec returns BOTH a non-empty and an empty
// result during a demo:
//   - Volunteers span all 3 approval statuses + at least one inactive.
//   - Opportunities straddle the 60-day "recent" line (some CreatedDate older than 60 days).
//   - Some volunteers/opportunities are matched; others aren't.
//
// LOCAL DEV admin login (seeded): username "admin", password "Admin2".
// This is throwaway dev-only credentials in a public repo — never reuse a real password here.
public static class DbSeeder
{
    public static void Seed(AppDbContext db)
    {
        db.Database.Migrate(); // apply any pending migrations before seeding

        if (db.Users.Any()) return; // already seeded — do nothing

        var hasher = new PasswordHasher<User>();

        // --- Admin login ---
        var admin = new User { Username = "admin", Role = UserRole.Admin };
        admin.PasswordHash = hasher.HashPassword(admin, "Admin2");
        db.Users.Add(admin);

        // --- Centers ---
        var north = new Center { Name = "North Community Center" };
        var south = new Center { Name = "South Community Center" };
        var riverside = new Center { Name = "Riverside Outreach" };
        db.Centers.AddRange(north, south, riverside);

        // --- Skills ---
        var driving = new Skill { Name = "Driving" };
        var cooking = new Skill { Name = "Cooking" };
        var tutoring = new Skill { Name = "Tutoring" };
        var firstAid = new Skill { Name = "First Aid" };
        var eventPlanning = new Skill { Name = "Event Planning" };
        var translation = new Skill { Name = "Translation" };
        db.Skills.AddRange(driving, cooking, tutoring, firstAid, eventPlanning, translation);

        db.SaveChanges(); // persist so FKs resolve

        // --- Volunteers (each with a linked User) ---
        // Spread across approval statuses and active/inactive so filters demo both ways.
        var volunteers = new List<Volunteer>
        {
            MakeVolunteer(hasher, "jsmith",  "John",   "Smith",    "john.smith@example.com",   ApprovalStatus.Approved,    true,  "904-555-0101"),
            MakeVolunteer(hasher, "mgarcia", "Maria",  "Garcia",   "maria.garcia@example.com", ApprovalStatus.Approved,    true,  "904-555-0102"),
            MakeVolunteer(hasher, "dlee",    "David",  "Lee",      "david.lee@example.com",    ApprovalStatus.Approved,    false, "904-555-0103"), // approved but INACTIVE
            MakeVolunteer(hasher, "swilson", "Sarah",  "Wilson",   "sarah.wilson@example.com", ApprovalStatus.Pending,     true,  "904-555-0104"),
            MakeVolunteer(hasher, "rbrown",  "Robert", "Brown",    "robert.brown@example.com", ApprovalStatus.Pending,     true,  null),
            MakeVolunteer(hasher, "kjones",  "Karen",  "Jones",    "karen.jones@example.com",  ApprovalStatus.Disapproved, true,  "904-555-0106"),
            MakeVolunteer(hasher, "tanderson","Tom",   "Anderson", "tom.anderson@example.com", ApprovalStatus.Approved,    true,  "904-555-0107"),
            MakeVolunteer(hasher, "lmartin", "Lisa",   "Martin",   "lisa.martin@example.com",  ApprovalStatus.Approved,    false, "904-555-0108"), // approved but INACTIVE
            MakeVolunteer(hasher, "cwhite",  "Chris",  "White",    "chris.white@example.com",  ApprovalStatus.Pending,     true,  "904-555-0109"),
            MakeVolunteer(hasher, "adavis",  "Amy",    "Davis",    "amy.davis@example.com",    ApprovalStatus.Approved,    true,  "904-555-0110"),
        };
        db.Volunteers.AddRange(volunteers);
        db.SaveChanges();

        // --- Volunteer <-> Center affiliations ---
        db.VolunteerCenters.AddRange(
            new VolunteerCenter { VolunteerId = volunteers[0].Id, CenterId = north.Id },
            new VolunteerCenter { VolunteerId = volunteers[1].Id, CenterId = north.Id },
            new VolunteerCenter { VolunteerId = volunteers[1].Id, CenterId = south.Id },
            new VolunteerCenter { VolunteerId = volunteers[3].Id, CenterId = south.Id },
            new VolunteerCenter { VolunteerId = volunteers[6].Id, CenterId = riverside.Id },
            new VolunteerCenter { VolunteerId = volunteers[9].Id, CenterId = north.Id }
        );

        // --- Volunteer <-> Skill ---
        db.VolunteerSkills.AddRange(
            new VolunteerSkill { VolunteerId = volunteers[0].Id, SkillId = driving.Id },
            new VolunteerSkill { VolunteerId = volunteers[0].Id, SkillId = firstAid.Id },
            new VolunteerSkill { VolunteerId = volunteers[1].Id, SkillId = cooking.Id },
            new VolunteerSkill { VolunteerId = volunteers[1].Id, SkillId = translation.Id },
            new VolunteerSkill { VolunteerId = volunteers[3].Id, SkillId = tutoring.Id },
            new VolunteerSkill { VolunteerId = volunteers[6].Id, SkillId = eventPlanning.Id },
            new VolunteerSkill { VolunteerId = volunteers[9].Id, SkillId = driving.Id }
        );

        // --- Opportunities: some recent (within 60 days), some older, to exercise the "recent" filter ---
        var now = DateTime.UtcNow;
        var opportunities = new List<Opportunity>
        {
            new() { Name = "Food Bank Sorting",     Description = "Sort and pack donated food.",          CenterId = north.Id,     StartDate = now.AddDays(7),   CreatedDate = now.AddDays(-3),  Location = "North Warehouse", VolunteersNeeded = 5, IsActive = true },
            new() { Name = "After-School Tutoring",  Description = "Help students with homework.",         CenterId = south.Id,     StartDate = now.AddDays(14),  CreatedDate = now.AddDays(-10), Location = "South Library",   VolunteersNeeded = 3, IsActive = true },
            new() { Name = "Community Garden Day",   Description = "Plant and maintain the garden.",       CenterId = riverside.Id, StartDate = now.AddDays(21),  CreatedDate = now.AddDays(-1),  Location = "Riverside Park",  VolunteersNeeded = 8, IsActive = true },
            new() { Name = "Holiday Meal Prep",      Description = "Prepare meals for the holiday drive.", CenterId = north.Id,     StartDate = now.AddDays(30),  CreatedDate = now.AddDays(-45), Location = "North Kitchen",   VolunteersNeeded = 10, IsActive = true },
            // Older than 60 days — should be EXCLUDED by the "Most Recent (60 days)" filter:
            new() { Name = "Summer Fundraiser",      Description = "Past fundraising event.",              CenterId = south.Id,     StartDate = now.AddDays(-20), CreatedDate = now.AddDays(-75), Location = "South Hall",      VolunteersNeeded = 6, IsActive = false },
            new() { Name = "Spring Cleanup",         Description = "Past neighborhood cleanup.",           CenterId = riverside.Id, StartDate = now.AddDays(-40), CreatedDate = now.AddDays(-90), Location = "Riverside",       VolunteersNeeded = 4, IsActive = false },
            new() { Name = "Blood Drive Support",    Description = "Assist at the blood drive.",           CenterId = north.Id,     StartDate = now.AddDays(10),  CreatedDate = now.AddDays(-5),  Location = "North Center",    VolunteersNeeded = 5, IsActive = true },
            new() { Name = "Senior Companion Visits",Description = "Visit and assist seniors.",            CenterId = south.Id,     StartDate = now.AddDays(5),   CreatedDate = now.AddDays(-15), Location = "Various",         VolunteersNeeded = 7, IsActive = true },
        };
        db.Opportunities.AddRange(opportunities);
        db.SaveChanges();

        // --- Matches: some volunteers assigned to some opportunities; leave several unmatched ---
        db.VolunteerOpportunities.AddRange(
            new VolunteerOpportunity { VolunteerId = volunteers[0].Id, OpportunityId = opportunities[0].Id, DateMatched = now.AddDays(-2) },
            new VolunteerOpportunity { VolunteerId = volunteers[1].Id, OpportunityId = opportunities[0].Id, DateMatched = now.AddDays(-2) },
            new VolunteerOpportunity { VolunteerId = volunteers[3].Id, OpportunityId = opportunities[1].Id, DateMatched = now.AddDays(-1) },
            new VolunteerOpportunity { VolunteerId = volunteers[6].Id, OpportunityId = opportunities[2].Id, DateMatched = now.AddDays(-1) },
            new VolunteerOpportunity { VolunteerId = volunteers[0].Id, OpportunityId = opportunities[6].Id, DateMatched = now }
        );
        db.SaveChanges();
    }

    private static Volunteer MakeVolunteer(
        PasswordHasher<User> hasher, string username, string first, string last,
        string email, ApprovalStatus status, bool isActive, string? cellPhone)
    {
        var user = new User { Username = username, Role = UserRole.Volunteer };
        user.PasswordHash = hasher.HashPassword(user, "Volunteer123!");

        return new Volunteer
        {
            User = user,
            FirstName = first,
            LastName = last,
            Email = email,
            CellPhone = cellPhone,
            ApprovalStatus = status,
            IsActive = isActive
        };
    }
}
