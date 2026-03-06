using Microsoft.EntityFrameworkCore;
using HealthCare.Data;
using HealthCare.Data.Models;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditEntity
{
    protected readonly HealthCareDbContext _context;

    public GenericRepository(HealthCareDbContext context)
    {
        _context = context;
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<T> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return await Task.FromResult(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity == null)
            return false;

        _context.Set<T>().Remove(entity);
        return true;
    }
}
