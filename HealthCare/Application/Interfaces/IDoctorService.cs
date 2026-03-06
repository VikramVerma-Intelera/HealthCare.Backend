using HealthCare.Application.DTOs;

namespace HealthCare.Application.Interfaces;

public interface IDoctorService
{
    Task<IEnumerable<DoctorDto>> GetAllAsync();
    Task<DoctorDto?> GetByIdAsync(int id);
    Task<DoctorDto> CreateAsync(CreateDoctorDto dto, string userId);
    Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto, string userId);
    Task<bool> DeleteAsync(int id);
}
