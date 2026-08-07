using Microsoft.EntityFrameworkCore;
using VolunteerMS.Data.Repositories.Interfaces;

namespace VolunteerMS.Data.Repositories;
public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual void UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual void DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }
}