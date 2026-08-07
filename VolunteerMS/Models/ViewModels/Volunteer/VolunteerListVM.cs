namespace VolunteerMS.Models.ViewModels.Volunteer;
public class VolunteerListVM
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public ApprovalStatus ApprovalStatus { get; set; }}