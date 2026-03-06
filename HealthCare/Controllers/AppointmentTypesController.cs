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
public class AppointmentTypesController : ControllerBase
{
    private readonly IAppointmentTypeService _appointmentTypeService;
    private readonly IValidator<CreateAppointmentTypeDto> _createValidator;
    private readonly IValidator<UpdateAppointmentTypeDto> _updateValidator;
    private readonly ILogger<AppointmentTypesController> _logger;

    public AppointmentTypesController(
        IAppointmentTypeService appointmentTypeService,
        IValidator<CreateAppointmentTypeDto> createValidator,
        IValidator<UpdateAppointmentTypeDto> updateValidator,
        ILogger<AppointmentTypesController> logger)
    {
        _appointmentTypeService = appointmentTypeService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AppointmentTypeDto>>>> GetAll()
    {
        try
        {
            var appointmentTypes = await _appointmentTypeService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<AppointmentTypeDto>>.SuccessResponse(appointmentTypes, "Appointment types retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointment types");
            return StatusCode(500, ApiResponse<IEnumerable<AppointmentTypeDto>>.ErrorResponse("An error occurred while retrieving appointment types", 500));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<AppointmentTypeDto>>> GetById(int id)
    {
        try
        {
            var appointmentType = await _appointmentTypeService.GetByIdAsync(id);
            if (appointmentType == null)
                return NotFound(ApiResponse<AppointmentTypeDto>.NotFoundResponse("Appointment type not found"));

            return Ok(ApiResponse<AppointmentTypeDto>.SuccessResponse(appointmentType, "Appointment type retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving appointment type with id {id}");
            return StatusCode(500, ApiResponse<AppointmentTypeDto>.ErrorResponse("An error occurred while retrieving the appointment type", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AppointmentTypeDto>>> Create([FromBody] CreateAppointmentTypeDto dto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<AppointmentTypeDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var appointmentType = await _appointmentTypeService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = appointmentType.Id }, ApiResponse<AppointmentTypeDto>.CreatedResponse(appointmentType));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment type");
            return StatusCode(500, ApiResponse<AppointmentTypeDto>.ErrorResponse("An error occurred while creating the appointment type", 500));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<AppointmentTypeDto>>> Update(int id, [FromBody] UpdateAppointmentTypeDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<AppointmentTypeDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var appointmentType = await _appointmentTypeService.UpdateAsync(id, dto, userId);
            return Ok(ApiResponse<AppointmentTypeDto>.SuccessResponse(appointmentType, "Appointment type updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<AppointmentTypeDto>.NotFoundResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating appointment type with id {id}");
            return StatusCode(500, ApiResponse<AppointmentTypeDto>.ErrorResponse("An error occurred while updating the appointment type", 500));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var result = await _appointmentTypeService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.NotFoundResponse("Appointment type not found"));

            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Appointment type deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting appointment type with id {id}");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the appointment type", 500));
        }
    }
}
