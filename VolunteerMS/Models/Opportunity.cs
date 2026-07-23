using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models;

// A volunteer opportunity, owned by a Center.
// IsActive means "closed/filled but kept on record" — distinct from deletion,
// which is a hard delete (see AppDbContext cascade config).
// "Most Recent (60 days)" filter keys off CreatedDate, not StartDate.
public class Opportunity
{
    public int Id { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(2000)]
    public string Description { get; set; } = string.Empty;

    // When the opportunity takes place.
    public DateTime StartDate { get; set; }

    // When the record was created — set automatically, drives the "recent" filter.
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    [MaxLength(200)]
    public string? Location { get; set; }

    public int VolunteersNeeded { get; set; }

    public bool IsActive { get; set; } = true;

    // --- Owning center (required) ---
    public int CenterId { get; set; }
    public Center Center { get; set; } = null!;

    // --- Matched volunteers ---
    public ICollection<VolunteerOpportunity> VolunteerOpportunities { get; set; } = new List<VolunteerOpportunity>();
}
