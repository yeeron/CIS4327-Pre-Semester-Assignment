using VolunteerMS.Models;

namespace VolunteerMS.Data.Repositories.Interfaces;
public interface IUserRepository : IGenericRepository<User>
{
    Task<User?> GetByUsernameAsync(string username);
}