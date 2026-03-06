using AutoMapper;
using HealthCare.Data.Models;
using HealthCare.Application.DTOs;

namespace HealthCare.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Department Mappings
        CreateMap<Department, DepartmentDto>().ReverseMap();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();

        // Patient Mappings
        CreateMap<Patient, PatientDto>().ReverseMap();
        CreateMap<CreatePatientDto, Patient>();
        CreateMap<UpdatePatientDto, Patient>();

        // PatientDetails Mappings
        CreateMap<PatientDetails, PatientDetailsDto>().ReverseMap();
        CreateMap<CreatePatientDetailsDto, PatientDetails>();
        CreateMap<UpdatePatientDetailsDto, PatientDetails>();

        // Doctor Mappings
        CreateMap<Doctor, DoctorDto>().ReverseMap();
        CreateMap<CreateDoctorDto, Doctor>();
        CreateMap<UpdateDoctorDto, Doctor>();

        // DoctorDetails Mappings
        CreateMap<DoctorDetails, DoctorDetailsDto>().ReverseMap();
        CreateMap<CreateDoctorDetailsDto, DoctorDetails>();
        CreateMap<UpdateDoctorDetailsDto, DoctorDetails>();

        // AppointmentType Mappings
        CreateMap<AppointmentType, AppointmentTypeDto>().ReverseMap();
        CreateMap<CreateAppointmentTypeDto, AppointmentType>();
        CreateMap<UpdateAppointmentTypeDto, AppointmentType>();

        // Appointment Mappings
        CreateMap<Appointment, AppointmentDto>().ReverseMap();
        CreateMap<CreateAppointmentDto, Appointment>();
        CreateMap<UpdateAppointmentDto, Appointment>();
    }
}
