using HealthCare.Application.DTOs;

namespace HealthCare.Application.Interfaces;

public interface IDoctorDetailsService
{
    Task<IEnumerable<DoctorDetailsDto>> GetAllAsync();
    Task<DoctorDetailsDto?> GetByIdAsync(int id);
    Task<DoctorDetailsDto> CreateAsync(CreateDoctorDetailsDto dto, string userId);
    Task<DoctorDetailsDto> UpdateAsync(int id, UpdateDoctorDetailsDto dto, string userId);
    Task<bool> DeleteAsync(int id);
}
