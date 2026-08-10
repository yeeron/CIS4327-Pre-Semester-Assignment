using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models.ViewModels.Volunteer;
public class VolunteerEditVM : VolunteerBaseVM
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    public string? Password { get; set; } = string.Empty;
    public ApprovalStatus ApprovalStatus { get; set; }
}