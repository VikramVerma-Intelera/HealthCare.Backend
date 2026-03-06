using HealthCare.Application.DTOs;

namespace HealthCare.Application.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<AppointmentDto>> GetAllAsync();
    Task<AppointmentDto?> GetByIdAsync(int id);
    Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, string userId);
    Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto, string userId);
    Task<bool> DeleteAsync(int id);
}
