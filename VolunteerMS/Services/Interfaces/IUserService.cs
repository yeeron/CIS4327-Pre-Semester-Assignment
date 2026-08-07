using VolunteerMS.Models;

namespace VolunteerMS.Services.Interfaces;
public interface IUserService
{
    Task<User?> GetByIdAsync(int id);

    Task<User?> GetByUsernameAsync(string username);

    //Task<bool> ValidateLoginAsync(string username, string password);
    //Task<User?> AuthenticateAsync(string username, string password);
}