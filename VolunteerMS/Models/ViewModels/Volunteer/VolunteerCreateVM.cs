using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models.ViewModels.Volunteer;
public class VolunteerCreateVM : VolunteerBaseVM
{
    [Required]
    [Display(Name = "Username")]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Password)]
    [StringLength(100)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "Approval Status")]
    public ApprovalStatus ApprovalStatus { get; set; }
        = ApprovalStatus.Pending;
}