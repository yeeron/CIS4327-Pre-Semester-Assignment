using System.ComponentModel.DataAnnotations;
namespace VolunteerMS.Utilities;
public enum VolunteerFilter
{
    [Display(Name = "Approved / Pending Approval")]
    Default,
    
    [Display(Name = "Pending Approval")]
    Pending,

    [Display(Name = "Approved")]
    Approved,

    [Display(Name = "Disapproved")]
    Disapproved,

    [Display(Name = "All")]
    All
}