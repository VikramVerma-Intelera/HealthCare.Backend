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
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IValidator<CreateAppointmentDto> _createValidator;
    private readonly IValidator<UpdateAppointmentDto> _updateValidator;
    private readonly ILogger<AppointmentsController> _logger;

    public AppointmentsController(
        IAppointmentService appointmentService,
        IValidator<CreateAppointmentDto> createValidator,
        IValidator<UpdateAppointmentDto> updateValidator,
        ILogger<AppointmentsController> logger)
    {
        _appointmentService = appointmentService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<AppointmentDto>>>> GetAll()
    {
        try
        {
            var appointments = await _appointmentService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<AppointmentDto>>.SuccessResponse(appointments, "Appointments retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving appointments");
            return StatusCode(500, ApiResponse<IEnumerable<AppointmentDto>>.ErrorResponse("An error occurred while retrieving appointments", 500));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<AppointmentDto>>> GetById(int id)
    {
        try
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null)
                return NotFound(ApiResponse<AppointmentDto>.NotFoundResponse("Appointment not found"));

            return Ok(ApiResponse<AppointmentDto>.SuccessResponse(appointment, "Appointment retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving appointment with id {id}");
            return StatusCode(500, ApiResponse<AppointmentDto>.ErrorResponse("An error occurred while retrieving the appointment", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<AppointmentDto>>> Create([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<AppointmentDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var appointment = await _appointmentService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id }, ApiResponse<AppointmentDto>.CreatedResponse(appointment));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating appointment");
            return StatusCode(500, ApiResponse<AppointmentDto>.ErrorResponse("An error occurred while creating the appointment", 500));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<AppointmentDto>>> Update(int id, [FromBody] UpdateAppointmentDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<AppointmentDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var appointment = await _appointmentService.UpdateAsync(id, dto, userId);
            return Ok(ApiResponse<AppointmentDto>.SuccessResponse(appointment, "Appointment updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<AppointmentDto>.NotFoundResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating appointment with id {id}");
            return StatusCode(500, ApiResponse<AppointmentDto>.ErrorResponse("An error occurred while updating the appointment", 500));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var result = await _appointmentService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.NotFoundResponse("Appointment not found"));

            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Appointment deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting appointment with id {id}");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the appointment", 500));
        }
    }
}
