using AutoMapper;
using Microsoft.Extensions.Logging;
using HealthCare.Data.Models;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DoctorService> _logger;

    public DoctorService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DoctorService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<DoctorDto>> GetAllAsync()
    {
        try
        {
            var doctors = await _unitOfWork.DoctorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDto>>(doctors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all doctors.");
            throw;
        }
    }

    public async Task<DoctorDto?> GetByIdAsync(int id)
    {
        try
        {
            var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);
            return doctor == null ? null : _mapper.Map<DoctorDto>(doctor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting doctor with id {id}.");
            throw;
        }
    }

    public async Task<DoctorDto> CreateAsync(CreateDoctorDto dto, string userId)
    {
        try
        {
            var doctor = _mapper.Map<Doctor>(dto);
            doctor.CreatedBy = userId;
            doctor.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.DoctorRepository.AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Doctor created with id {doctor.Id}.");
            return _mapper.Map<DoctorDto>(doctor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating doctor.");
            throw;
        }
    }

    public async Task<DoctorDto> UpdateAsync(int id, UpdateDoctorDto dto, string userId)
    {
        try
        {
            var doctor = await _unitOfWork.DoctorRepository.GetByIdAsync(id);
            if (doctor == null)
                throw new KeyNotFoundException($"Doctor with id {id} not found.");

            _mapper.Map(dto, doctor);
            doctor.ModifiedBy = userId;
            doctor.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.DoctorRepository.UpdateAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Doctor with id {id} updated.");
            return _mapper.Map<DoctorDto>(doctor);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating doctor with id {id}.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var result = await _unitOfWork.DoctorRepository.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Doctor with id {id} deleted.");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting doctor with id {id}.");
            throw;
        }
    }
}
