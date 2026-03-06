using AutoMapper;
using Microsoft.Extensions.Logging;
using HealthCare.Data.Models;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Services;

public class PatientService : IPatientService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<PatientService> _logger;

    public PatientService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PatientService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<PatientDto>> GetAllAsync()
    {
        try
        {
            var patients = await _unitOfWork.PatientRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PatientDto>>(patients);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all patients.");
            throw;
        }
    }

    public async Task<PatientDto?> GetByIdAsync(int id)
    {
        try
        {
            var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
            return patient == null ? null : _mapper.Map<PatientDto>(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting patient with id {id}.");
            throw;
        }
    }

    public async Task<PatientDto> CreateAsync(CreatePatientDto dto, string userId)
    {
        try
        {
            var patient = _mapper.Map<Patient>(dto);
            patient.CreatedBy = userId;
            patient.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.PatientRepository.AddAsync(patient);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Patient created with id {patient.Id}.");
            return _mapper.Map<PatientDto>(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient.");
            throw;
        }
    }

    public async Task<PatientDto> UpdateAsync(int id, UpdatePatientDto dto, string userId)
    {
        try
        {
            var patient = await _unitOfWork.PatientRepository.GetByIdAsync(id);
            if (patient == null)
                throw new KeyNotFoundException($"Patient with id {id} not found.");

            _mapper.Map(dto, patient);
            patient.ModifiedBy = userId;
            patient.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.PatientRepository.UpdateAsync(patient);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Patient with id {id} updated.");
            return _mapper.Map<PatientDto>(patient);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating patient with id {id}.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var result = await _unitOfWork.PatientRepository.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Patient with id {id} deleted.");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting patient with id {id}.");
            throw;
        }
    }
}
