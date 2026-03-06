using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;
using HealthCare.Application.Common;

namespace HealthCare.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PatientDetailsController : ControllerBase
{
    private readonly IPatientDetailsService _patientDetailsService;
    private readonly ILogger<PatientDetailsController> _logger;

    public PatientDetailsController(
        IPatientDetailsService patientDetailsService,
        ILogger<PatientDetailsController> logger)
    {
        _patientDetailsService = patientDetailsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PatientDetailsDto>>>> GetAll()
    {
        try
        {
            var patientDetails = await _patientDetailsService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<PatientDetailsDto>>.SuccessResponse(patientDetails, "Patient details retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patient details");
            return StatusCode(500, ApiResponse<IEnumerable<PatientDetailsDto>>.ErrorResponse("An error occurred while retrieving patient details", 500));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PatientDetailsDto>>> GetById(int id)
    {
        try
        {
            var patientDetails = await _patientDetailsService.GetByIdAsync(id);
            if (patientDetails == null)
                return NotFound(ApiResponse<PatientDetailsDto>.NotFoundResponse("Patient details not found"));

            return Ok(ApiResponse<PatientDetailsDto>.SuccessResponse(patientDetails, "Patient details retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving patient details with id {id}");
            return StatusCode(500, ApiResponse<PatientDetailsDto>.ErrorResponse("An error occurred while retrieving the patient details", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PatientDetailsDto>>> Create([FromBody] CreatePatientDetailsDto dto)
    {
        try
        {
            var userId = User?.Identity?.Name ?? "System";
            var patientDetails = await _patientDetailsService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = patientDetails.Id }, ApiResponse<PatientDetailsDto>.CreatedResponse(patientDetails));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient details");
            return StatusCode(500, ApiResponse<PatientDetailsDto>.ErrorResponse("An error occurred while creating the patient details", 500));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PatientDetailsDto>>> Update(int id, [FromBody] UpdatePatientDetailsDto dto)
    {
        try
        {
            var userId = User?.Identity?.Name ?? "System";
            var patientDetails = await _patientDetailsService.UpdateAsync(id, dto, userId);
            return Ok(ApiResponse<PatientDetailsDto>.SuccessResponse(patientDetails, "Patient details updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<PatientDetailsDto>.NotFoundResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating patient details with id {id}");
            return StatusCode(500, ApiResponse<PatientDetailsDto>.ErrorResponse("An error occurred while updating the patient details", 500));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var result = await _patientDetailsService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.NotFoundResponse("Patient details not found"));

            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Patient details deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting patient details with id {id}");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the patient details", 500));
        }
    }
}
