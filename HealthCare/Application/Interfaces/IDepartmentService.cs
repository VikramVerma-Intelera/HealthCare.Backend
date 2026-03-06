using HealthCare.Application.DTOs;

namespace HealthCare.Application.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();
    Task<DepartmentDto?> GetByIdAsync(int id);
    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, string userId);
    Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto dto, string userId);
    Task<bool> DeleteAsync(int id);
}
