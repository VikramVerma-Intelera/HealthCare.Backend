using AutoMapper;
using Microsoft.Extensions.Logging;
using HealthCare.Data.Models;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Services;

public class PatientDetailsService : IPatientDetailsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PatientDetailsService> _logger;

    public PatientDetailsService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PatientDetailsService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PatientDetailsDto>> GetAllAsync()
    {
        try
        {
            var patientDetails = await _unitOfWork.PatientDetailsRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDetailsDto>>(patientDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all patient details.");
            throw;
        }
    }

    public async Task<PatientDetailsDto?> GetByIdAsync(int id)
    {
        try
        {
            var patientDetails = await _unitOfWork.PatientDetailsRepository.GetByIdAsync(id);
            return patientDetails == null ? null : _mapper.Map<PatientDetailsDto>(patientDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting patient details with id {id}.");
            throw;
        }
    }

    public async Task<PatientDetailsDto> CreateAsync(CreatePatientDetailsDto dto, string userId)
    {
        try
        {
            var patientDetails = _mapper.Map<PatientDetails>(dto);
            patientDetails.CreatedBy = userId;
            patientDetails.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.PatientDetailsRepository.AddAsync(patientDetails);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Patient details created with id {patientDetails.Id}.");
            return _mapper.Map<PatientDetailsDto>(patientDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient details.");
            throw;
        }
    }

    public async Task<PatientDetailsDto> UpdateAsync(int id, UpdatePatientDetailsDto dto, string userId)
    {
        try
        {
            var patientDetails = await _unitOfWork.PatientDetailsRepository.GetByIdAsync(id);
            if (patientDetails == null)
                throw new KeyNotFoundException($"Patient details with id {id} not found.");

            _mapper.Map(dto, patientDetails);
            patientDetails.ModifiedBy = userId;
            patientDetails.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.PatientDetailsRepository.UpdateAsync(patientDetails);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Patient details with id {id} updated.");
            return _mapper.Map<PatientDetailsDto>(patientDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating patient details with id {id}.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var result = await _unitOfWork.PatientDetailsRepository.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Patient details with id {id} deleted.");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting patient details with id {id}.");
            throw;
        }
    }
}
