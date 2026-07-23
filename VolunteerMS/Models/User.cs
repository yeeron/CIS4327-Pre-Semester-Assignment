using System.ComponentModel.DataAnnotations;

namespace VolunteerMS.Models;

public enum UserRole
{
    Admin = 0,
    Volunteer = 1
}

// Login identity. Admins log in; volunteers have a User row (the spec gives
// volunteer profiles username/password fields) but never log in — login checks Role == Admin.
public class User
{
    public int Id { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    // 1:1 back-reference to a volunteer profile, if this user is a volunteer.
    public Volunteer? Volunteer { get; set; }
}
