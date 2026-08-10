using VolunteerMS.Data.Repositories.Interfaces;
using VolunteerMS.Services.Interfaces;
using VolunteerMS.UnitOfWorks.Interfaces;

namespace VolunteerMS.Services;
public abstract class GenericService<T> : IGenericService<T>
    where T : class
{
    protected readonly IGenericRepository<T> _repository;
    protected readonly IUnitOfWork UnitOfWork;

    protected GenericService(
        IGenericRepository<T> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        UnitOfWork = unitOfWork;
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public virtual async Task<T> AddAsync(T entity)
    {
        await _repository.AddAsync(entity);

        await UnitOfWork.SaveChangesAsync();

        return entity;
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _repository.UpdateAsync(entity);

        await UnitOfWork.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            return;

        _repository.DeleteAsync(entity);

        await UnitOfWork.SaveChangesAsync();
    }
}