namespace VolunteerMS.Models.ViewModels.Center;
public class CenterIndexVM
{
    public string? SearchTerm { get; set; }
    public List<CenterListVM> Centers { get; set; } = new();
}