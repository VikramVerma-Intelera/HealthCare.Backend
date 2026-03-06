using Xunit;
using Moq;
using FluentAssertions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using HealthCare.Data.Models;
using HealthCare.Application.DTOs;
using HealthCare.Application.Interfaces;
using HealthCare.Infrastructure.Services;

namespace HealthCare.Tests.Services;

public class DepartmentServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<DepartmentService>> _mockLogger;
    private readonly DepartmentService _service;

    public DepartmentServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<DepartmentService>>();
        _service = new DepartmentService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfDepartments()
    {
        // Arrange
        var departments = new List<Department>
        {
            new Department { Id = 1, DepartmentName = "Cardiology", DepartmentCode = "CARD", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Department { Id = 2, DepartmentName = "Neurology", DepartmentCode = "NEUR", CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        };

        var departmentDtos = new List<DepartmentDto>
        {
            new DepartmentDto { Id = 1, DepartmentName = "Cardiology", DepartmentCode = "CARD" },
            new DepartmentDto { Id = 2, DepartmentName = "Neurology", DepartmentCode = "NEUR" }
        };

        _mockUnitOfWork.Setup(u => u.DepartmentRepository.GetAllAsync()).ReturnsAsync(departments);
        _mockMapper.Setup(m => m.Map<IEnumerable<DepartmentDto>>(It.IsAny<IEnumerable<Department>>())).Returns(departmentDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _mockUnitOfWork.Verify(u => u.DepartmentRepository.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnDepartment()
    {
        // Arrange
        var departmentId = 1;
        var department = new Department { Id = departmentId, DepartmentName = "Cardiology", DepartmentCode = "CARD", CreatedBy = "System", CreatedOn = DateTime.UtcNow };
        var departmentDto = new DepartmentDto { Id = departmentId, DepartmentName = "Cardiology", DepartmentCode = "CARD" };

        _mockUnitOfWork.Setup(u => u.DepartmentRepository.GetByIdAsync(departmentId)).ReturnsAsync(department);
        _mockMapper.Setup(m => m.Map<DepartmentDto>(It.IsAny<Department>())).Returns(departmentDto);

        // Act
        var result = await _service.GetByIdAsync(departmentId);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(departmentId);
        _mockUnitOfWork.Verify(u => u.DepartmentRepository.GetByIdAsync(departmentId), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithInvalidId_ShouldReturnNull()
    {
        // Arrange
        var departmentId = 999;
        _mockUnitOfWork.Setup(u => u.DepartmentRepository.GetByIdAsync(departmentId)).ReturnsAsync((Department?)null);

        // Act
        var result = await _service.GetByIdAsync(departmentId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateDepartment()
    {
        // Arrange
        var createDto = new CreateDepartmentDto { DepartmentName = "Cardiology", DepartmentCode = "CARD", Description = "Heart Care" };
        var department = new Department { Id = 1, DepartmentName = "Cardiology", DepartmentCode = "CARD", CreatedBy = "TestUser", CreatedOn = DateTime.UtcNow };
        var departmentDto = new DepartmentDto { Id = 1, DepartmentName = "Cardiology", DepartmentCode = "CARD" };

        _mockMapper.Setup(m => m.Map<Department>(It.IsAny<CreateDepartmentDto>())).Returns(department);
        _mockUnitOfWork.Setup(u => u.DepartmentRepository.AddAsync(It.IsAny<Department>())).ReturnsAsync(department);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<DepartmentDto>(It.IsAny<Department>())).Returns(departmentDto);

        // Act
        var result = await _service.CreateAsync(createDto, "TestUser");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        _mockUnitOfWork.Verify(u => u.DepartmentRepository.AddAsync(It.IsAny<Department>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateDepartment()
    {
        // Arrange
        var departmentId = 1;
        var updateDto = new UpdateDepartmentDto { DepartmentName = "Updated", DepartmentCode = "UPD", Description = "Updated Dept" };
        var department = new Department { Id = departmentId, DepartmentName = "Old", DepartmentCode = "OLD", CreatedBy = "System", CreatedOn = DateTime.UtcNow };
        var departmentDto = new DepartmentDto { Id = departmentId, DepartmentName = "Updated", DepartmentCode = "UPD" };

        _mockUnitOfWork.Setup(u => u.DepartmentRepository.GetByIdAsync(departmentId)).ReturnsAsync(department);
        _mockMapper.Setup(m => m.Map(It.IsAny<UpdateDepartmentDto>(), It.IsAny<Department>()));
        _mockUnitOfWork.Setup(u => u.DepartmentRepository.UpdateAsync(It.IsAny<Department>())).ReturnsAsync(department);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<DepartmentDto>(It.IsAny<Department>())).Returns(departmentDto);

        // Act
        var result = await _service.UpdateAsync(departmentId, updateDto, "TestUser");

        // Assert
        result.Should().NotBeNull();
        _mockUnitOfWork.Verify(u => u.DepartmentRepository.UpdateAsync(It.IsAny<Department>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteDepartment()
    {
        // Arrange
        var departmentId = 1;
        _mockUnitOfWork.Setup(u => u.DepartmentRepository.DeleteAsync(departmentId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(departmentId);

        // Assert
        result.Should().BeTrue();
        _mockUnitOfWork.Verify(u => u.DepartmentRepository.DeleteAsync(departmentId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithInvalidId_ShouldReturnFalse()
    {
        // Arrange
        var departmentId = 999;
        _mockUnitOfWork.Setup(u => u.DepartmentRepository.DeleteAsync(departmentId)).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(departmentId);

        // Assert
        result.Should().BeFalse();
    }
}
