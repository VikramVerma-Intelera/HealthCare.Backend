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
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    private readonly IValidator<CreatePatientDto> _createValidator;
    private readonly IValidator<UpdatePatientDto> _updateValidator;
    private readonly ILogger<PatientsController> _logger;

    public PatientsController(
        IPatientService patientService,
        IValidator<CreatePatientDto> createValidator,
        IValidator<UpdatePatientDto> updateValidator,
        ILogger<PatientsController> logger)
    {
        _patientService = patientService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<PatientDto>>>> GetAll()
    {
        try
        {
            var patients = await _patientService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<PatientDto>>.SuccessResponse(patients, "Patients retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving patients");
            return StatusCode(500, ApiResponse<IEnumerable<PatientDto>>.ErrorResponse("An error occurred while retrieving patients", 500));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<PatientDto>>> GetById(int id)
    {
        try
        {
            var patient = await _patientService.GetByIdAsync(id);
            if (patient == null)
                return NotFound(ApiResponse<PatientDto>.NotFoundResponse("Patient not found"));

            return Ok(ApiResponse<PatientDto>.SuccessResponse(patient, "Patient retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving patient with id {id}");
            return StatusCode(500, ApiResponse<PatientDto>.ErrorResponse("An error occurred while retrieving the patient", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<PatientDto>>> Create([FromBody] CreatePatientDto dto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<PatientDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var patient = await _patientService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, ApiResponse<PatientDto>.CreatedResponse(patient));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating patient");
            return StatusCode(500, ApiResponse<PatientDto>.ErrorResponse("An error occurred while creating the patient", 500));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<PatientDto>>> Update(int id, [FromBody] UpdatePatientDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<PatientDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var patient = await _patientService.UpdateAsync(id, dto, userId);
            return Ok(ApiResponse<PatientDto>.SuccessResponse(patient, "Patient updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<PatientDto>.NotFoundResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating patient with id {id}");
            return StatusCode(500, ApiResponse<PatientDto>.ErrorResponse("An error occurred while updating the patient", 500));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var result = await _patientService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.NotFoundResponse("Patient not found"));

            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Patient deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting patient with id {id}");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the patient", 500));
        }
    }
}
