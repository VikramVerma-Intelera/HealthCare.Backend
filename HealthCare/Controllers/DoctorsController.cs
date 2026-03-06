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
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    private readonly IValidator<CreateDoctorDto> _createValidator;
    private readonly IValidator<UpdateDoctorDto> _updateValidator;
    private readonly ILogger<DoctorsController> _logger;

    public DoctorsController(
        IDoctorService doctorService,
        IValidator<CreateDoctorDto> createValidator,
        IValidator<UpdateDoctorDto> updateValidator,
        ILogger<DoctorsController> logger)
    {
        _doctorService = doctorService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<DoctorDto>>>> GetAll()
    {
        try
        {
            var doctors = await _doctorService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<DoctorDto>>.SuccessResponse(doctors, "Doctors retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctors");
            return StatusCode(500, ApiResponse<IEnumerable<DoctorDto>>.ErrorResponse("An error occurred while retrieving doctors", 500));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DoctorDto>>> GetById(int id)
    {
        try
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            if (doctor == null)
                return NotFound(ApiResponse<DoctorDto>.NotFoundResponse("Doctor not found"));

            return Ok(ApiResponse<DoctorDto>.SuccessResponse(doctor, "Doctor retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving doctor with id {id}");
            return StatusCode(500, ApiResponse<DoctorDto>.ErrorResponse("An error occurred while retrieving the doctor", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DoctorDto>>> Create([FromBody] CreateDoctorDto dto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<DoctorDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var doctor = await _doctorService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, ApiResponse<DoctorDto>.CreatedResponse(doctor));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating doctor");
            return StatusCode(500, ApiResponse<DoctorDto>.ErrorResponse("An error occurred while creating the doctor", 500));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DoctorDto>>> Update(int id, [FromBody] UpdateDoctorDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<DoctorDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var doctor = await _doctorService.UpdateAsync(id, dto, userId);
            return Ok(ApiResponse<DoctorDto>.SuccessResponse(doctor, "Doctor updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<DoctorDto>.NotFoundResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating doctor with id {id}");
            return StatusCode(500, ApiResponse<DoctorDto>.ErrorResponse("An error occurred while updating the doctor", 500));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var result = await _doctorService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.NotFoundResponse("Doctor not found"));

            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Doctor deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting doctor with id {id}");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the doctor", 500));
        }
    }
}
