using HealthCare.Data;
using HealthCare.Data.Models;
using HealthCare.Application.Interfaces;
using HealthCare.Infrastructure.Repositories;

namespace HealthCare.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly HealthCareDbContext _context;

    private IGenericRepository<Patient>? _patientRepository;
    private IGenericRepository<PatientDetails>? _patientDetailsRepository;
    private IGenericRepository<Department>? _departmentRepository;
    private IGenericRepository<Doctor>? _doctorRepository;
    private IGenericRepository<DoctorDetails>? _doctorDetailsRepository;
    private IGenericRepository<AppointmentType>? _appointmentTypeRepository;
    private IGenericRepository<Appointment>? _appointmentRepository;

    public UnitOfWork(HealthCareDbContext context)
    {
        _context = context;
    }

    public IGenericRepository<Patient> PatientRepository =>
        _patientRepository ??= new GenericRepository<Patient>(_context);

    public IGenericRepository<PatientDetails> PatientDetailsRepository =>
        _patientDetailsRepository ??= new GenericRepository<PatientDetails>(_context);

    public IGenericRepository<Department> DepartmentRepository =>
        _departmentRepository ??= new GenericRepository<Department>(_context);

    public IGenericRepository<Doctor> DoctorRepository =>
        _doctorRepository ??= new GenericRepository<Doctor>(_context);

    public IGenericRepository<DoctorDetails> DoctorDetailsRepository =>
        _doctorDetailsRepository ??= new GenericRepository<DoctorDetails>(_context);

    public IGenericRepository<AppointmentType> AppointmentTypeRepository =>
        _appointmentTypeRepository ??= new GenericRepository<AppointmentType>(_context);

    public IGenericRepository<Appointment> AppointmentRepository =>
        _appointmentRepository ??= new GenericRepository<Appointment>(_context);

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
