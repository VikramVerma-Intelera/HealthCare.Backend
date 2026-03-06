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
public class DepartmentsController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly IValidator<CreateDepartmentDto> _createValidator;
    private readonly IValidator<UpdateDepartmentDto> _updateValidator;
    private readonly ILogger<DepartmentsController> _logger;

    public DepartmentsController(
        IDepartmentService departmentService,
        IValidator<CreateDepartmentDto> createValidator,
        IValidator<UpdateDepartmentDto> updateValidator,
        ILogger<DepartmentsController> logger)
    {
        _departmentService = departmentService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<DepartmentDto>>>> GetAll()
    {
        try
        {
            var departments = await _departmentService.GetAllAsync();
            return Ok(ApiResponse<IEnumerable<DepartmentDto>>.SuccessResponse(departments, "Departments retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving departments");
            return StatusCode(500, ApiResponse<IEnumerable<DepartmentDto>>.ErrorResponse("An error occurred while retrieving departments", 500));
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> GetById(int id)
    {
        try
        {
            var department = await _departmentService.GetByIdAsync(id);
            if (department == null)
                return NotFound(ApiResponse<DepartmentDto>.NotFoundResponse("Department not found"));

            return Ok(ApiResponse<DepartmentDto>.SuccessResponse(department, "Department retrieved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving department with id {id}");
            return StatusCode(500, ApiResponse<DepartmentDto>.ErrorResponse("An error occurred while retrieving the department", 500));
        }
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> Create([FromBody] CreateDepartmentDto dto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<DepartmentDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var department = await _departmentService.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, ApiResponse<DepartmentDto>.CreatedResponse(department));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating department");
            return StatusCode(500, ApiResponse<DepartmentDto>.ErrorResponse("An error occurred while creating the department", 500));
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<DepartmentDto>>> Update(int id, [FromBody] UpdateDepartmentDto dto)
    {
        try
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
                return BadRequest(ApiResponse<DepartmentDto>.ErrorResponse($"Validation failed: {errors}", 400));
            }

            var userId = User?.Identity?.Name ?? "System";
            var department = await _departmentService.UpdateAsync(id, dto, userId);
            return Ok(ApiResponse<DepartmentDto>.SuccessResponse(department, "Department updated successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<DepartmentDto>.NotFoundResponse(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating department with id {id}");
            return StatusCode(500, ApiResponse<DepartmentDto>.ErrorResponse("An error occurred while updating the department", 500));
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id)
    {
        try
        {
            var result = await _departmentService.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<object>.NotFoundResponse("Department not found"));

            return Ok(ApiResponse<object>.SuccessResponse(new { }, "Department deleted successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting department with id {id}");
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while deleting the department", 500));
        }
    }
}
