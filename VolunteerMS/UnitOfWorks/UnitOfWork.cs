
using VolunteerMS.Data;
using VolunteerMS.Data.Repositories.Interfaces;
using VolunteerMS.UnitOfWorks.Interfaces;

namespace VolunteerMS.UnitOfWorks;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public IUserRepository Users { get; }
    public IVolunteerRepository Volunteers { get; }
    public ISkillRepository Skills { get; }
    public ICenterRepository Centers { get; }
    public IOpportunityRepository Opportunities { get; }

    public UnitOfWork(AppDbContext context, IUserRepository userRepository, IVolunteerRepository volunteerRepository,
    ISkillRepository skillRepository, ICenterRepository centerRepository, IOpportunityRepository opportunityRepository)
    {
        _context = context;
        Users = userRepository; //same as Users = new UserRepository(_context);
        Volunteers = volunteerRepository;
        Skills = skillRepository;
        Centers = centerRepository;
        Opportunities = opportunityRepository;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}