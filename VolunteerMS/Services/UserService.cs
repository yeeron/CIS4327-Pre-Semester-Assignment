using VolunteerMS.Models;
using VolunteerMS.Services.Interfaces;
using VolunteerMS.UnitOfWorks.Interfaces;

namespace VolunteerMS.Services;
public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<User?> GetByIdAsync(int id)
    {
        return await _unitOfWork.Users.GetByIdAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _unitOfWork.Users.GetByUsernameAsync(username);
    }


    //This method is commented out because it was replaced by the AuthenticateAsync method below, which returns the user object 
    // if authentication is successful, or null if it fails. This allows for more flexibility in handling authenticated users. 
    // This return only a boolean value indicating whether the login was successful or not, which may not be sufficient for all use cases.
    /*public async Task<bool> ValidateLoginAsync(string username, string password)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(username);

        if (user == null)
            return false;

        return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
    }*/

    // This method authenticates a user by their username and password. It retrieves the user from the database using the provided username, 
    // and then verifies the provided password against the stored password hash using BCrypt. 
    // If the authentication is successful, it returns the user object; otherwise, it returns null. 
    // This allows for more flexibility in handling authenticated users, as the calling code can access the authenticated user's details if needed.
    /*public async Task<User?> AuthenticateAsync(string username, string password)
    {
        var user = await _unitOfWork.Users.GetByUsernameAsync(username);

        if (user == null)
            return null;

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        return isPasswordValid ? user : null;
    }*/
}