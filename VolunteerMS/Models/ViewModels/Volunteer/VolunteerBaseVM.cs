using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models.ViewModels.Volunteer;
public class VolunteerBaseVM
{
    //Personal Information
    [Required]
    [Display(Name = "First Name")]
    [StringLength(50)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Last Name")]
    [StringLength(50)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Address")]
    [StringLength(200)]
    public string? Address { get; set; }

    //Phone Numbers
    [Phone]
    [StringLength(20)]
    public string? HomePhone { get; set; }

    [Phone]
    [StringLength(20)]
    public string? WorkPhone { get; set; }

    [Phone]
    [StringLength(20)]
    public string? CellPhone { get; set; }

    
    //Education and Licenses
    [Display(Name = "Educational Background")]
    [StringLength(200)]
    public string? EducationalBackground { get; set; }

    [Display(Name = "Current Licenses")]
    [StringLength(200)]
    public string? CurrentLicenses { get; set; }

    [Display(Name = "Availability")]
    [StringLength(200)]
    public string? Availability { get; set; }


    //Emergency Contact Information
    [Display(Name = "Emergency Contact")]
    [StringLength(100)]
    public string? EmergencyContactName { get; set; }

    [Phone]
    [Display(Name = "Emergency Contact Phone")]
    [StringLength(20)]
    public string? EmergencyContactPhone { get; set; }

    [EmailAddress]
    [Display(Name = "Emergency Contact Email")]
    [StringLength(100)]
    public string? EmergencyContactEmail { get; set; }

    [Display(Name = "Emergency Contact Address")]
    [StringLength(200)]
    public string? EmergencyContactAddress { get; set; }

    //Documents: Additional Information
    [Display(Name = "Driver's License On File")]
    public bool DriverLicenseOnFile { get; set; }

    [Display(Name = "Social Security Card On File")]
    public bool SSCardOnFile { get; set; }
}