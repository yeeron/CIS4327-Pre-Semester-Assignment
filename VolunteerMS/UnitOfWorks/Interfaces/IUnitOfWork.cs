using VolunteerMS.Data.Repositories.Interfaces;

namespace VolunteerMS.UnitOfWorks.Interfaces;
public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IVolunteerRepository Volunteers { get; }
    ISkillRepository Skills { get; }
    ICenterRepository Centers { get; }
    IOpportunityRepository Opportunities { get; }

    Task<int> SaveChangesAsync();
}