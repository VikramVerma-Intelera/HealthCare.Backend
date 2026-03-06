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

public class PatientServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<PatientService>> _mockLogger;
    private readonly PatientService _service;

    public PatientServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PatientService>>();
        _service = new PatientService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfPatients()
    {
        // Arrange
        var patients = new List<Patient>
        {
            new Patient { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "555-0101", DateOfBirth = new DateTime(1980, 5, 15), Gender = "Male", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Patient { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", PhoneNumber = "555-0102", DateOfBirth = new DateTime(1990, 8, 22), Gender = "Female", CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        };

        var patientDtos = new List<PatientDto>
        {
            new PatientDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            new PatientDto { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
        };

        _mockUnitOfWork.Setup(u => u.PatientRepository.GetAllAsync()).ReturnsAsync(patients);
        _mockMapper.Setup(m => m.Map<IEnumerable<PatientDto>>(It.IsAny<IEnumerable<Patient>>())).Returns(patientDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _mockUnitOfWork.Verify(u => u.PatientRepository.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnPatient()
    {
        // Arrange
        var patientId = 1;
        var patient = new Patient { Id = patientId, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "555-0101", DateOfBirth = new DateTime(1980, 5, 15), Gender = "Male", CreatedBy = "System", CreatedOn = DateTime.UtcNow };
        var patientDto = new PatientDto { Id = patientId, FirstName = "John", LastName = "Doe", Email = "john@example.com" };

        _mockUnitOfWork.Setup(u => u.PatientRepository.GetByIdAsync(patientId)).ReturnsAsync(patient);
        _mockMapper.Setup(m => m.Map<PatientDto>(It.IsAny<Patient>())).Returns(patientDto);

        // Act
        var result = await _service.GetByIdAsync(patientId);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(patientId);
        _mockUnitOfWork.Verify(u => u.PatientRepository.GetByIdAsync(patientId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreatePatient()
    {
        // Arrange
        var createDto = new CreatePatientDto { FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "555-0101", DateOfBirth = new DateTime(1980, 5, 15), Gender = "Male" };
        var patient = new Patient { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com", PhoneNumber = "555-0101", DateOfBirth = new DateTime(1980, 5, 15), Gender = "Male", CreatedBy = "TestUser", CreatedOn = DateTime.UtcNow };
        var patientDto = new PatientDto { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" };

        _mockMapper.Setup(m => m.Map<Patient>(It.IsAny<CreatePatientDto>())).Returns(patient);
        _mockUnitOfWork.Setup(u => u.PatientRepository.AddAsync(It.IsAny<Patient>())).ReturnsAsync(patient);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<PatientDto>(It.IsAny<Patient>())).Returns(patientDto);

        // Act
        var result = await _service.CreateAsync(createDto, "TestUser");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        _mockUnitOfWork.Verify(u => u.PatientRepository.AddAsync(It.IsAny<Patient>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeletePatient()
    {
        // Arrange
        var patientId = 1;
        _mockUnitOfWork.Setup(u => u.PatientRepository.DeleteAsync(patientId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(patientId);

        // Assert
        result.Should().BeTrue();
        _mockUnitOfWork.Verify(u => u.PatientRepository.DeleteAsync(patientId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
