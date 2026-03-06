using AutoMapper;
using Microsoft.Extensions.Logging;
using HealthCare.Data.Models;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Services;

public class DoctorDetailsService : IDoctorDetailsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DoctorDetailsService> _logger;

    public DoctorDetailsService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DoctorDetailsService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<DoctorDetailsDto>> GetAllAsync()
    {
        try
        {
            var doctorDetails = await _unitOfWork.DoctorDetailsRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DoctorDetailsDto>>(doctorDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all doctor details.");
            throw;
        }
    }

    public async Task<DoctorDetailsDto?> GetByIdAsync(int id)
    {
        try
        {
            var doctorDetails = await _unitOfWork.DoctorDetailsRepository.GetByIdAsync(id);
            return doctorDetails == null ? null : _mapper.Map<DoctorDetailsDto>(doctorDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting doctor details with id {id}.");
            throw;
        }
    }

    public async Task<DoctorDetailsDto> CreateAsync(CreateDoctorDetailsDto dto, string userId)
    {
        try
        {
            var doctorDetails = _mapper.Map<DoctorDetails>(dto);
            doctorDetails.CreatedBy = userId;
            doctorDetails.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.DoctorDetailsRepository.AddAsync(doctorDetails);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Doctor details created with id {doctorDetails.Id}.");
            return _mapper.Map<DoctorDetailsDto>(doctorDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating doctor details.");
            throw;
        }
    }

    public async Task<DoctorDetailsDto> UpdateAsync(int id, UpdateDoctorDetailsDto dto, string userId)
    {
        try
        {
            var doctorDetails = await _unitOfWork.DoctorDetailsRepository.GetByIdAsync(id);
            if (doctorDetails == null)
                throw new KeyNotFoundException($"Doctor details with id {id} not found.");

            _mapper.Map(dto, doctorDetails);
            doctorDetails.ModifiedBy = userId;
            doctorDetails.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.DoctorDetailsRepository.UpdateAsync(doctorDetails);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Doctor details with id {id} updated.");
            return _mapper.Map<DoctorDetailsDto>(doctorDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating doctor details with id {id}.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var result = await _unitOfWork.DoctorDetailsRepository.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Doctor details with id {id} deleted.");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting doctor details with id {id}.");
            throw;
        }
    }
}
