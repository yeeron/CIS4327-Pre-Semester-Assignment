using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models;

public enum UserRole
{
    Admin = 0,
    Volunteer = 1
}

// will finish soonTM
public class User
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }
}