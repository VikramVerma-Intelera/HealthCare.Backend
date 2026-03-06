using HealthCare.Data.Models;

namespace HealthCare.Application.Interfaces;

public interface IGenericRepository<T> where T : BaseAuditEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task<T> UpdateAsync(T entity);
    Task<bool> DeleteAsync(int id);
}
