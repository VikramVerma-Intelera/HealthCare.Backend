using Microsoft.EntityFrameworkCore;
using HealthCare.Data.Models;

namespace HealthCare.Data;

public class HealthCareDbContext : DbContext
{
    public HealthCareDbContext(DbContextOptions<HealthCareDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients { get; set; }
    public DbSet<PatientDetails> PatientDetails { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<DoctorDetails> DoctorDetails { get; set; }
    public DbSet<AppointmentType> AppointmentTypes { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure decimal properties
        modelBuilder.Entity<AppointmentType>()
            .Property(x => x.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<DoctorDetails>()
            .Property(x => x.ConsultationFee)
            .HasPrecision(18, 2);

        // Patient - PatientDetails (One to One)
        modelBuilder.Entity<Patient>()
            .HasOne(p => p.PatientDetails)
            .WithOne(pd => pd.Patient)
            .HasForeignKey<PatientDetails>(pd => pd.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        // Doctor - DoctorDetails (One to One)
        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.DoctorDetails)
            .WithOne(dd => dd.Doctor)
            .HasForeignKey<DoctorDetails>(dd => dd.DoctorId)
            .OnDelete(DeleteBehavior.Cascade);

        // Department - Doctor (One to Many)
        modelBuilder.Entity<Doctor>()
            .HasOne(d => d.Department)
            .WithMany(dept => dept.Doctors)
            .HasForeignKey(d => d.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Patient - Appointment (One to Many)
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Doctor - Appointment (One to Many)
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Doctor)
            .WithMany(d => d.Appointments)
            .HasForeignKey(a => a.DoctorId)
            .OnDelete(DeleteBehavior.Restrict);

        // AppointmentType - Appointment (One to Many)
        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.AppointmentType)
            .WithMany(at => at.Appointments)
            .HasForeignKey(a => a.AppointmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed Departments
        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, DepartmentName = "Primary Care", DepartmentCode = "PC", Description = "General healthcare services", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Department { Id = 2, DepartmentName = "Specialist Care", DepartmentCode = "SC", Description = "Specialized healthcare services", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Department { Id = 3, DepartmentName = "Virtual Care", DepartmentCode = "VC", Description = "Telemedicine and virtual consultations", CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        );

        // Seed AppointmentTypes
        modelBuilder.Entity<AppointmentType>().HasData(
            new AppointmentType { Id = 1, TypeName = "General Consultation", TypeCode = "GC", Department = "Primary Care", Description = "Comprehensive health checkup and consultation with experienced physicians", DurationInMinutes = 30, Price = 75, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new AppointmentType { Id = 2, TypeName = "Cardiology Checkup", TypeCode = "CC", Department = "Specialist Care", Description = "Heart health assessment with ECG and expert consultation", DurationInMinutes = 45, Price = 150, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new AppointmentType { Id = 3, TypeName = "Dermatology Consultation", TypeCode = "DC", Department = "Specialist Care", Description = "Skin care consultation and treatment planning", DurationInMinutes = 30, Price = 100, CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new AppointmentType { Id = 4, TypeName = "Telemedicine Visit", TypeCode = "TV", Department = "Virtual Care", Description = "Online video consultation with healthcare professionals", DurationInMinutes = 20, Price = 50, CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        );

        // Seed Patients
        modelBuilder.Entity<Patient>().HasData(
            new Patient { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", PhoneNumber = "555-0101", DateOfBirth = new DateTime(1980, 5, 15), Gender = "Male", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Patient { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com", PhoneNumber = "555-0102", DateOfBirth = new DateTime(1990, 8, 22), Gender = "Female", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Patient { Id = 3, FirstName = "Michael", LastName = "Johnson", Email = "michael.j@example.com", PhoneNumber = "555-0103", DateOfBirth = new DateTime(1975, 3, 10), Gender = "Male", CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        );

        // Seed PatientDetails
        modelBuilder.Entity<PatientDetails>().HasData(
            new PatientDetails { Id = 1, PatientId = 1, Address = "123 Main St", City = "New York", State = "NY", PostalCode = "10001", BloodType = "O+", EmergencyContactName = "Mary Doe", EmergencyContactPhone = "555-0104", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new PatientDetails { Id = 2, PatientId = 2, Address = "456 Oak Ave", City = "Boston", State = "MA", PostalCode = "02101", BloodType = "A+", EmergencyContactName = "John Smith", EmergencyContactPhone = "555-0105", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new PatientDetails { Id = 3, PatientId = 3, Address = "789 Pine Rd", City = "Chicago", State = "IL", PostalCode = "60601", BloodType = "B+", EmergencyContactName = "Sarah Johnson", EmergencyContactPhone = "555-0106", CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        );

        // Seed Doctors
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor { Id = 1, FirstName = "Sarah", LastName = "Johnson", Email = "sarah.johnson@hospital.com", PhoneNumber = "555-1001", LicenseNumber = "MD-001", DepartmentId = 2, Speciality = "Cardiology", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Doctor { Id = 2, FirstName = "Michael", LastName = "Chen", Email = "michael.chen@hospital.com", PhoneNumber = "555-1002", LicenseNumber = "MD-002", DepartmentId = 1, Speciality = "General Practice", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Doctor { Id = 3, FirstName = "Emily", LastName = "Brown", Email = "emily.brown@hospital.com", PhoneNumber = "555-1003", LicenseNumber = "MD-003", DepartmentId = 2, Speciality = "Dermatology", CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        );

        // Seed DoctorDetails
        modelBuilder.Entity<DoctorDetails>().HasData(
            new DoctorDetails { Id = 1, DoctorId = 1, QualificationDetails = "MD, Board Certified in Cardiology", Experience = "15 years", ConsultationFee = 150, ConsultationDuration = "45 min", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new DoctorDetails { Id = 2, DoctorId = 2, QualificationDetails = "MD, General Practice", Experience = "10 years", ConsultationFee = 75, ConsultationDuration = "30 min", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new DoctorDetails { Id = 3, DoctorId = 3, QualificationDetails = "MD, Board Certified in Dermatology", Experience = "12 years", ConsultationFee = 100, ConsultationDuration = "30 min", CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        );

        // Seed Appointments
        modelBuilder.Entity<Appointment>().HasData(
            new Appointment { Id = 1, PatientId = 1, DoctorId = 1, AppointmentTypeId = 2, AppointmentDate = DateTime.UtcNow.AddDays(5), AppointmentTime = DateTime.UtcNow.AddDays(5).AddHours(10), Status = "Confirmed", Notes = "Annual checkup", Location = "Medical Center, Building A, Room 302", CreatedBy = "System", CreatedOn = DateTime.UtcNow },
            new Appointment { Id = 2, PatientId = 2, DoctorId = 2, AppointmentTypeId = 1, AppointmentDate = DateTime.UtcNow.AddDays(7), AppointmentTime = DateTime.UtcNow.AddDays(7).AddHours(14), Status = "Scheduled", Notes = "Follow-up appointment", Location = "Online Consultation", CreatedBy = "System", CreatedOn = DateTime.UtcNow }
        );
    }
}
