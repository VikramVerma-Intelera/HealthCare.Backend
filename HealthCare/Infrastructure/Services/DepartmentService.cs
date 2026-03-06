using AutoMapper;
using Microsoft.Extensions.Logging;
using HealthCare.Data.Models;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;

namespace HealthCare.Infrastructure.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<DepartmentService> _logger;

    public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        try
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<DepartmentDto>>(departments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all departments.");
            throw;
        }
    }

    public async Task<DepartmentDto?> GetByIdAsync(int id)
    {
        try
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            return department == null ? null : _mapper.Map<DepartmentDto>(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting department with id {id}.");
            throw;
        }
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto, string userId)
    {
        try
        {
            var department = _mapper.Map<Department>(dto);
            department.CreatedBy = userId;
            department.CreatedOn = DateTime.UtcNow;

            await _unitOfWork.DepartmentRepository.AddAsync(department);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Department created with id {department.Id}.");
            return _mapper.Map<DepartmentDto>(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department.");
            throw;
        }
    }

    public async Task<DepartmentDto> UpdateAsync(int id, UpdateDepartmentDto dto, string userId)
    {
        try
        {
            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id);
            if (department == null)
                throw new KeyNotFoundException($"Department with id {id} not found.");

            _mapper.Map(dto, department);
            department.ModifiedBy = userId;
            department.ModifiedOn = DateTime.UtcNow;

            await _unitOfWork.DepartmentRepository.UpdateAsync(department);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Department with id {id} updated.");
            return _mapper.Map<DepartmentDto>(department);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating department with id {id}.");
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            var result = await _unitOfWork.DepartmentRepository.DeleteAsync(id);
            if (result)
            {
                await _unitOfWork.SaveChangesAsync();
                _logger.LogInformation($"Department with id {id} deleted.");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting department with id {id}.");
            throw;
        }
    }
}
