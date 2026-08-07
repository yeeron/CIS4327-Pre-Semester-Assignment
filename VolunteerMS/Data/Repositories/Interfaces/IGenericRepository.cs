namespace VolunteerMS.Data.Repositories.Interfaces;
public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();

    Task<T?> GetByIdAsync(int id);

    Task AddAsync(T entity);

    void UpdateAsync(T entity);

    void DeleteAsync(T entity);
}