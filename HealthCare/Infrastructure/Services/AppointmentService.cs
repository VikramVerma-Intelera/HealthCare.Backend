using AutoMapper;
using Microsoft.Extensions.Logging;
using HealthCare.Data.Models;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentService> _logger;

    public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AppointmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAllAsync()
    {
        try
        {
            var appointments = await _unitOfWork.AppointmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all appointments.");
            throw;
        }
    }

    public async Task<AppointmentDto?> GetByIdAsync(int id)
    {
        try
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            return appointment == null ? null : _mapper.Map<AppointmentDto>(appointment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting appointment with id {id}.");
            throw;
        }
    }

    public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, string userId)
    {
        try
        {
            var appointment = _mapper.Map<Appointment>(dto);
            appointment.Status = "Scheduled";
            appointment.CreatedBy = userId;
            appointment.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.AppointmentRepository.AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Appointment created with id {appointment.Id}.");
            return _mapper.Map<AppointmentDto>(appointment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment.");
            throw;
        }
    }

    public async Task<AppointmentDto> UpdateAsync(int id, UpdateAppointmentDto dto, string userId)
    {
        try
        {
            var appointment = await _unitOfWork.AppointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with id {id} not found.");

            _mapper.Map(dto, appointment);
            appointment.ModifiedBy = userId;
            appointment.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.AppointmentRepository.UpdateAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Appointment with id {id} updated.");
            return _mapper.Map<AppointmentDto>(appointment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating appointment with id {id}.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var result = await _unitOfWork.AppointmentRepository.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Appointment with id {id} deleted.");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting appointment with id {id}.");
            throw;
        }
    }
}
