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

public class AppointmentServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<AppointmentService>> _mockLogger;
    private readonly AppointmentService _service;

    public AppointmentServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<AppointmentService>>();
        _service = new AppointmentService(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnListOfAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>
        {
            new Appointment { Id = 1, PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, AppointmentDate = DateTime.UtcNow.AddDays(5), AppointmentTime = DateTime.UtcNow.AddDays(5).AddHours(10), Status = "Scheduled", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Appointment { Id = 2, PatientId = 2, DoctorId = 2, AppointmentTypeId = 2, AppointmentDate = DateTime.UtcNow.AddDays(7), AppointmentTime = DateTime.UtcNow.AddDays(7).AddHours(14), Status = "Scheduled", CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        };

        var appointmentDtos = new List<AppointmentDto>
        {
            new AppointmentDto { Id = 1, PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, Status = "Scheduled" },
            new AppointmentDto { Id = 2, PatientId = 2, DoctorId = 2, AppointmentTypeId = 2, Status = "Scheduled" }
        };

        _mockUnitOfWork.Setup(u => u.AppointmentRepository.GetAllAsync()).ReturnsAsync(appointments);
        _mockMapper.Setup(m => m.Map<IEnumerable<AppointmentDto>>(It.IsAny<IEnumerable<Appointment>>())).Returns(appointmentDtos);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(2);
        _mockUnitOfWork.Verify(u => u.AppointmentRepository.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ShouldReturnAppointment()
    {
        // Arrange
        var appointmentId = 1;
        var appointment = new Appointment { Id = appointmentId, PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, AppointmentDate = DateTime.UtcNow.AddDays(5), AppointmentTime = DateTime.UtcNow.AddDays(5).AddHours(10), Status = "Scheduled", CreatedBy = "System", CreatedOn = DateTime.UtcNow };
        var appointmentDto = new AppointmentDto { Id = appointmentId, PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, Status = "Scheduled" };

        _mockUnitOfWork.Setup(u => u.AppointmentRepository.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
        _mockMapper.Setup(m => m.Map<AppointmentDto>(It.IsAny<Appointment>())).Returns(appointmentDto);

        // Act
        var result = await _service.GetByIdAsync(appointmentId);

        // Assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(appointmentId);
        _mockUnitOfWork.Verify(u => u.AppointmentRepository.GetByIdAsync(appointmentId), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateAppointment()
    {
        // Arrange
        var createDto = new CreateAppointmentDto { PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, AppointmentDate = DateTime.UtcNow.AddDays(5), AppointmentTime = DateTime.UtcNow.AddDays(5).AddHours(10) };
        var appointment = new Appointment { Id = 1, PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, AppointmentDate = DateTime.UtcNow.AddDays(5), AppointmentTime = DateTime.UtcNow.AddDays(5).AddHours(10), Status = "Scheduled", CreatedBy = "TestUser", CreatedOn = DateTime.UtcNow };
        var appointmentDto = new AppointmentDto { Id = 1, PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, Status = "Scheduled" };

        _mockMapper.Setup(m => m.Map<Appointment>(It.IsAny<CreateAppointmentDto>())).Returns(appointment);
        _mockUnitOfWork.Setup(u => u.AppointmentRepository.AddAsync(It.IsAny<Appointment>())).ReturnsAsync(appointment);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<AppointmentDto>(It.IsAny<Appointment>())).Returns(appointmentDto);

        // Act
        var result = await _service.CreateAsync(createDto, "TestUser");

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.Status.Should().Be("Scheduled");
        _mockUnitOfWork.Verify(u => u.AppointmentRepository.AddAsync(It.IsAny<Appointment>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateAppointment()
    {
        // Arrange
        var appointmentId = 1;
        var updateDto = new UpdateAppointmentDto { PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, AppointmentDate = DateTime.UtcNow.AddDays(5), AppointmentTime = DateTime.UtcNow.AddDays(5).AddHours(10), Status = "Confirmed" };
        var appointment = new Appointment { Id = appointmentId, PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, AppointmentDate = DateTime.UtcNow.AddDays(5), AppointmentTime = DateTime.UtcNow.AddDays(5).AddHours(10), Status = "Scheduled", CreatedBy = "System", CreatedOn = DateTime.UtcNow };
        var appointmentDto = new AppointmentDto { Id = appointmentId, PatientId = 1, DoctorId = 1, AppointmentTypeId = 1, Status = "Confirmed" };

        _mockUnitOfWork.Setup(u => u.AppointmentRepository.GetByIdAsync(appointmentId)).ReturnsAsync(appointment);
        _mockMapper.Setup(m => m.Map(It.IsAny<UpdateAppointmentDto>(), It.IsAny<Appointment>()));
        _mockUnitOfWork.Setup(u => u.AppointmentRepository.UpdateAsync(It.IsAny<Appointment>())).ReturnsAsync(appointment);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);
        _mockMapper.Setup(m => m.Map<AppointmentDto>(It.IsAny<Appointment>())).Returns(appointmentDto);

        // Act
        var result = await _service.UpdateAsync(appointmentId, updateDto, "TestUser");

        // Assert
        result.Should().NotBeNull();
        _mockUnitOfWork.Verify(u => u.AppointmentRepository.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteAppointment()
    {
        // Arrange
        var appointmentId = 1;
        _mockUnitOfWork.Setup(u => u.AppointmentRepository.DeleteAsync(appointmentId)).ReturnsAsync(true);
        _mockUnitOfWork.Setup(u => u.SaveChangesAsync()).Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(appointmentId);

        // Assert
        result.Should().BeTrue();
        _mockUnitOfWork.Verify(u => u.AppointmentRepository.DeleteAsync(appointmentId), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
    }
}
