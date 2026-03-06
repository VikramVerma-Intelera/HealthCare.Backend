using AutoMapper;
using Microsoft.Extensions.Logging;
using HealthCare.Data.Models;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Services;

public class AppointmentTypeService : IAppointmentTypeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AppointmentTypeService> _logger;

    public AppointmentTypeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AppointmentTypeService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<AppointmentTypeDto>> GetAllAsync()
    {
        try
        {
            var appointmentTypes = await _unitOfWork.AppointmentTypeRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AppointmentTypeDto>>(appointmentTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all appointment types.");
            throw;
        }
    }

    public async Task<AppointmentTypeDto?> GetByIdAsync(int id)
    {
        try
        {
            var appointmentType = await _unitOfWork.AppointmentTypeRepository.GetByIdAsync(id);
            return appointmentType == null ? null : _mapper.Map<AppointmentTypeDto>(appointmentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting appointment type with id {id}.");
            throw;
        }
    }

    public async Task<AppointmentTypeDto> CreateAsync(CreateAppointmentTypeDto dto, string userId)
    {
        try
        {
            var appointmentType = _mapper.Map<AppointmentType>(dto);
            appointmentType.CreatedBy = userId;
            appointmentType.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.AppointmentTypeRepository.AddAsync(appointmentType);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Appointment type created with id {appointmentType.Id}.");
            return _mapper.Map<AppointmentTypeDto>(appointmentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment type.");
            throw;
        }
    }

    public async Task<AppointmentTypeDto> UpdateAsync(int id, UpdateAppointmentTypeDto dto, string userId)
    {
        try
        {
            var appointmentType = await _unitOfWork.AppointmentTypeRepository.GetByIdAsync(id);
            if (appointmentType == null)
                throw new KeyNotFoundException($"Appointment type with id {id} not found.");

            _mapper.Map(dto, appointmentType);
            appointmentType.ModifiedBy = userId;
            appointmentType.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.AppointmentTypeRepository.UpdateAsync(appointmentType);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Appointment type with id {id} updated.");
            return _mapper.Map<AppointmentTypeDto>(appointmentType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating appointment type with id {id}.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var result = await _unitOfWork.AppointmentTypeRepository.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Appointment type with id {id} deleted.");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting appointment type with id {id}.");
            throw;
        }
    }
}
