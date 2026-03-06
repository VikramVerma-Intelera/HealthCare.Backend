using HealthCare.Data.Models;

namespace HealthCare.Application.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<Patient> PatientRepository { get; }
    IGenericRepository<PatientDetails> PatientDetailsRepository { get; }
    IGenericRepository<Department> DepartmentRepository { get; }
    IGenericRepository<Doctor> DoctorRepository { get; }
    IGenericRepository<DoctorDetails> DoctorDetailsRepository { get; }
    IGenericRepository<AppointmentType> AppointmentTypeRepository { get; }
    IGenericRepository<Appointment> AppointmentRepository { get; }

    Task SaveChangesAsync();
}
