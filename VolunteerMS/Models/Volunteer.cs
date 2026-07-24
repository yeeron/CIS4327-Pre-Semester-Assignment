using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models;

public enum ApprovalStatus
{
    Pending = 0,
    Approved = 1,
    Disapproved = 2
}

// Volunteer profile. Credentials live on the linked User row (1:1).
// ApprovalStatus and IsActive are independent: an Approved volunteer can become Inactive.
// Only FirstName/LastName/Email are required here; username/password are required on User.
public class Volunteer
{
    public int Id { get; set; }

    // --- Link to login identity (1:1, enforced by a unique index in AppDbContext) ---
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // --- Required identity fields ---
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    // --- Contact (all optional) ---
    [Phone, MaxLength(20)]
    public string? HomePhone { get; set; }

    [Phone, MaxLength(20)]
    public string? CellPhone { get; set; }

    [MaxLength(200)]
    public string? Address { get; set; }

    // --- Background (all optional) ---
    [MaxLength(500)]
    public string? EducationalBackground { get; set; }

    [MaxLength(500)]
    public string? Licenses { get; set; }

    [MaxLength(500)]
    public string? Availability { get; set; }

    // --- Emergency contact (all optional) ---
    [MaxLength(200)]
    public string? EmergencyContactName { get; set; }

    [Phone, MaxLength(20)]
    public string? EmergencyContactPhone { get; set; }

    [EmailAddress, MaxLength(200)]
    public string? EmergencyContactEmail { get; set; }

    [MaxLength(200)]
    public string? EmergencyContactAddress { get; set; }

    // --- Document-on-file tracking (whether copies are collected — NOT the documents/numbers) ---
    public bool DriverLicenseOnFile { get; set; }
    public bool SSCardOnFile { get; set; }

    // --- Status (independent axes) ---
    public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
    public bool IsActive { get; set; } = true;

    // --- Relationships ---
    public ICollection<VolunteerCenter> VolunteerCenters { get; set; } = new List<VolunteerCenter>();
    public ICollection<VolunteerSkill> VolunteerSkills { get; set; } = new List<VolunteerSkill>();
    public ICollection<VolunteerOpportunity> VolunteerOpportunities { get; set; } = new List<VolunteerOpportunity>();
}
