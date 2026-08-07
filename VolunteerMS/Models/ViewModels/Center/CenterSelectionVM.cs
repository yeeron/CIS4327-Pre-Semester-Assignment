namespace VolunteerMS.Models.ViewModels.Center;
public class CenterSelectionVM
{
    public int CenterId { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool Selected { get; set; }
}