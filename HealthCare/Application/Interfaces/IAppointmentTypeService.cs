using HealthCare.Application.DTOs;

namespace HealthCare.Application.Interfaces;

public interface IAppointmentTypeService
{
    Task<IEnumerable<AppointmentTypeDto>> GetAllAsync();
    Task<AppointmentTypeDto?> GetByIdAsync(int id);
    Task<AppointmentTypeDto> CreateAsync(CreateAppointmentTypeDto dto, string userId);
    Task<AppointmentTypeDto> UpdateAsync(int id, UpdateAppointmentTypeDto dto, string userId);
    Task<bool> DeleteAsync(int id);
}
