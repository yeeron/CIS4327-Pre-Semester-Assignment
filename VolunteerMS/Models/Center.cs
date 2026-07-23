using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models;

// An organizational center. Opportunities belong to one; volunteers can be affiliated with many
public class Center
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
    public ICollection<VolunteerCenter> VolunteerCenters { get; set; } = new List<VolunteerCenter>();
}
