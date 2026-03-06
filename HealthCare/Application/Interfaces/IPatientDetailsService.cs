using HealthCare.Application.DTOs;

namespace HealthCare.Application.Interfaces;

public interface IPatientDetailsService
{
    Task<IEnumerable<PatientDetailsDto>> GetAllAsync();
    Task<PatientDetailsDto?> GetByIdAsync(int id);
    Task<PatientDetailsDto> CreateAsync(CreatePatientDetailsDto dto, string userId);
    Task<PatientDetailsDto> UpdateAsync(int id, UpdatePatientDetailsDto dto, string userId);
    Task<bool> DeleteAsync(int id);
}
