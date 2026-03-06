using HealthCare.Application.DTOs;

namespace HealthCare.Application.Interfaces;

public interface IPatientService
{
    Task<IEnumerable<PatientDto>> GetAllAsync();
    Task<PatientDto?> GetByIdAsync(int id);
    Task<PatientDto> CreateAsync(CreatePatientDto dto, string userId);
    Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto, string userId);
    Task<bool> DeleteAsync(int id);
}
