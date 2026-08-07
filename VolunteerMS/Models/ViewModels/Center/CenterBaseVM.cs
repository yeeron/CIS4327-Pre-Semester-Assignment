using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models.ViewModels.Center;
public class CenterBaseVM
{
    [Required]
    [Display(Name = "Center Name")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}