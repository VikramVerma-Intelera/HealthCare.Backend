using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;
using HealthCare.Application.Common;

namespace HealthCare.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DoctorDetailsController : ControllerBase
{
    private readonly IDoctorDetailsService _doctorDetailsService;
    private readonly ILogger<DoctorDetailsController> _logger;

    public DoctorDetailsController(
        IDoctorDetailsService doctorDetailsService,
        ILogger<DoctorDetailsController> logger)
    {
        _doctorDetailsService = doctorDetailsService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<DoctorDetailsDto>>>> GetAll()
    {
        try
        {
            var doctorDetails = await _doctorDetailsService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<DoctorDetailsDto>>.SuccessResponse(doctorDetails, "Doctor details retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving doctor details");
            return StatusCode(500, ApiResponse<IEnumerable<DoctorDetailsDto>>.ErrorResponse("An error occurred while retrieving doctor details", 500));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DoctorDetailsDto>>> GetById(int id)
    {
        try
        {
            var doctorDetails = await _doctorDetailsService.GetByIdAsync(id);
            if (doctorDetails == null)
                return NotFound(ApiResponse<DoctorDetailsDto>.NotFoundResponse("Doctor details not found"));

            return Ok(ApiResponse<DoctorDetailsDto>.SuccessResponse(doctorDetails, "Doctor details retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving doctor details with id {id}");
            return StatusCode(500, ApiResponse<DoctorDetailsDto>.ErrorResponse("An error occurred while retrieving the doctor details", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DoctorDetailsDto>>> Create([FromBody] CreateDoctorDetailsDto dto)
    {
        try
        {
            var userId = User?.Identity?.Name ?? "System";
            var doctorDetails = await _doctorDetailsService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = doctorDetails.Id }, ApiResponse<DoctorDetailsDto>.CreatedResponse(doctorDetails));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating doctor details");
            return StatusCode(500, ApiResponse<DoctorDetailsDto>.ErrorResponse("An error occurred while creating the doctor details", 500));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DoctorDetailsDto>>> Update(int id, [FromBody] UpdateDoctorDetailsDto dto)
    {
        try
        {
            var userId = User?.Identity?.Name ?? "System";
            var doctorDetails = await _doctorDetailsService.UpdateAsync(id, dto, userId);
            return Ok(ApiResponse<DoctorDetailsDto>.SuccessResponse(doctorDetails, "Doctor details updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<DoctorDetailsDto>.NotFoundResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating doctor details with id {id}");
            return StatusCode(500, ApiResponse<DoctorDetailsDto>.ErrorResponse("An error occurred while updating the doctor details", 500));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var result = await _doctorDetailsService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.NotFoundResponse("Doctor details not found"));

            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Doctor details deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting doctor details with id {id}");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the doctor details", 500));
        }
    }
}
