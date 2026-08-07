using Microsoft.EntityFrameworkCore;
using VolunteerMS.Data.Repositories.Interfaces;
using VolunteerMS.Models;

namespace VolunteerMS.Data.Repositories;
public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        //return await _dbSet.FirstOrDefaultAsync(u => u.Username == username); //Same thing as above, but using _dbSet instead of _context.Users
    }
}