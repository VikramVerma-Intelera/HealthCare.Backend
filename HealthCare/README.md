# Enterprise Healthcare Web Application - .NET 8 Web API

A comprehensive healthcare management system built with .NET 8 following Clean Architecture principles. This application provides a complete solution for managing patients, doctors, departments, and medical appointments.

## 📋 Table of Contents

- [Architecture Overview](#architecture-overview)
- [Database Schema](#database-schema)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Endpoints](#api-endpoints)
- [Configuration](#configuration)
- [Running Tests](#running-tests)
- [Features](#features)

## 🏗️ Architecture Overview

This project follows **Clean Architecture** principles with the following layers:

```
Controllers → Services → Repository Pattern → Database Context
   ↓             ↓              ↓                   ↓
 HTTP         Business        Data Access      Entity Framework
Handlers       Logic           Operations        Core (Code-First)
```

### Layer Responsibilities:

- **Controllers**: Handle HTTP requests/responses and validation
- **Services**: Contain business logic with dependency injection
- **Repositories**: Data access abstraction using Repository Pattern with Unit of Work
- **Database**: Entity Framework Core with code-first migrations
- **DTOs**: Data Transfer Objects for request/response mapping with AutoMapper

## 📊 Database Schema

### Entity Relationship Diagram

```
Department (1) ←→ (Many) Doctor
                     ↓
                 DoctorDetails (1:1)

Patient (1) ←→ (Many) Appointment
                     ↓
                 PatientDetails (1:1)

AppointmentType (1) ←→ (Many) Appointment

Appointment connects: Patient, Doctor, AppointmentType
```

### Entities Overview

#### 1. **Department**
Represents medical departments (Primary Care, Specialist Care, Virtual Care)
- Id (Primary Key)
- DepartmentName
- DepartmentCode
- Description
- IsActive
- Audit Fields: CreatedBy, CreatedOn, ModifiedBy, ModifiedOn

#### 2. **Doctor**
Core doctor information
- Id (Primary Key)
- FirstName, LastName
- Email, PhoneNumber
- LicenseNumber
- DepartmentId (Foreign Key)
- Speciality
- IsActive
- Relationships: Department (Many-to-One), DoctorDetails (One-to-One), Appointments (One-to-Many)
- Audit Fields

#### 3. **DoctorDetails**
Extended doctor information
- Id (Primary Key)
- DoctorId (Foreign Key - One-to-One)
- QualificationDetails
- Experience
- ConsultationFee
- ConsultationDuration
- ClinicAddress
- Bio
- Audit Fields

#### 4. **Patient**
Core patient information
- Id (Primary Key)
- FirstName, LastName
- Email, PhoneNumber
- DateOfBirth
- Gender
- IsActive
- Relationships: PatientDetails (One-to-One), Appointments (One-to-Many)
- Audit Fields

#### 5. **PatientDetails**
Extended patient information
- Id (Primary Key)
- PatientId (Foreign Key - One-to-One)
- Address, City, State, PostalCode
- BloodType
- EmergencyContactName, EmergencyContactPhone
- AllergiesDescription
- MedicalHistoryNotes
- Audit Fields

#### 6. **AppointmentType**
Available appointment types with pricing
- Id (Primary Key)
- TypeName (e.g., "General Consultation", "Cardiology Checkup")
- TypeCode
- Department
- Description
- DurationInMinutes
- Price
- IsActive
- Relationships: Appointments (One-to-Many)
- Audit Fields

**Seeded Types:**
- General Consultation: $75, 30 min, Primary Care
- Cardiology Checkup: $150, 45 min, Specialist Care
- Dermatology Consultation: $100, 30 min, Specialist Care
- Telemedicine Visit: $50, 20 min, Virtual Care

#### 7. **Appointment**
Appointment bookings linking patients, doctors, and appointment types
- Id (Primary Key)
- PatientId (Foreign Key)
- DoctorId (Foreign Key)
- AppointmentTypeId (Foreign Key)
- AppointmentDate
- AppointmentTime
- Status (Scheduled, Confirmed, Completed, Cancelled)
- Notes
- Location
- Audit Fields

### Audit Fields

All entities extend `BaseAuditEntity` with:
- `CreatedBy`: User who created the record
- `CreatedOn`: UTC timestamp of creation
- `ModifiedBy`: Last user who modified (nullable)
- `ModifiedOn`: Last modification timestamp (nullable)

## 🛠️ Technology Stack

### Backend Framework
- **.NET 8** - Latest LTS version
- **ASP.NET Core 8** - Web API framework

### Database
- **SQL Server (Local/Remote)** - Primary data store
- **Entity Framework Core 8** - ORM with code-first approach
- **LocalDB** - Local development database

### Libraries & Patterns
- **AutoMapper** - DTO mapping
- **FluentValidation** - Request validation
- **Moq** - Unit test mocking
- **xUnit** - Testing framework
- **FluentAssertions** - Fluent test assertions

### Architectural Patterns
- **Clean Architecture** - Layered separation of concerns
- **Repository Pattern** - Data access abstraction
- **Unit of Work Pattern** - Transaction management
- **Dependency Injection** - Built-in .NET Core DI
- **Data Transfer Objects (DTOs)** - API contract definition

## 📁 Project Structure

```
HealthCare/
├── Controllers/                          # HTTP Endpoints
│   ├── DepartmentsController.cs
│   ├── PatientsController.cs
│   ├── PatientDetailsController.cs
│   ├── DoctorsController.cs
│   ├── DoctorDetailsController.cs
│   ├── AppointmentTypesController.cs
│   └── AppointmentsController.cs
│
├── Data/                                 # Database Layer
│   ├── Models/                          # Entity definitions
│   │   ├── BaseAuditEntity.cs           # Base class with audit fields
│   │   ├── Department.cs
│   │   ├── Doctor.cs
│   │   ├── DoctorDetails.cs
│   │   ├── Patient.cs
│   │   ├── PatientDetails.cs
│   │   ├── AppointmentType.cs
│   │   └── Appointment.cs
│   └── HealthCareDbContext.cs           # EF Core DbContext
│
├── Application/                          # Business Logic Layer
│   ├── DTOs/                            # Data Transfer Objects
│   │   ├── DepartmentDto.cs
│   │   ├── PatientDto.cs
│   │   ├── DoctorDto.cs
│   │   ├── AppointmentDto.cs
│   │   └── ...
│   │
│   ├── Interfaces/                      # Service & Repository Contracts
│   │   ├── IGenericRepository.cs
│   │   ├── IUnitOfWork.cs
│   │   ├── IDepartmentService.cs
│   │   ├── IPatientService.cs
│   │   └── ...
│   │
│   ├── Validators/                      # FluentValidation rules
│   │   ├── DepartmentValidator.cs
│   │   ├── PatientValidator.cs
│   │   ├── DoctorValidator.cs
│   │   ├── AppointmentValidator.cs
│   │   └── ...
│   │
│   ├── Mappings/                        # AutoMapper profiles
│   │   └── MappingProfile.cs
│   │
│   └── Common/                          # Shared utilities
│       └── ApiResponse.cs               # Standard response wrapper
│
├── Infrastructure/                       # Infrastructure Layer
│   ├── Repositories/
│   │   └── GenericRepository.cs         # Generic CRUD repository
│   │
│   ├── UnitOfWork/
│   │   └── UnitOfWork.cs                # Unit of Work implementation
│   │
│   └── Services/                        # Service implementations
│       ├── DepartmentService.cs
│       ├── PatientService.cs
│       ├── DoctorService.cs
│       ├── AppointmentService.cs
│       └── ...
│
├── Tests/                                # Unit Tests
│   └── Services/
│       ├── DepartmentServiceTests.cs
│       ├── PatientServiceTests.cs
│       └── AppointmentServiceTests.cs
│
├── Migrations/                           # EF Core Migrations (Auto-generated)
│
├── appsettings.Development.json         # Development configuration
├── appsettings.Production.json          # Production configuration
├── Program.cs                            # DI & Middleware setup
└── README.md                             # This file
```

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK or later
- SQL Server (LocalDB or Express)
- Visual Studio 2022 or VS Code
- Git

### Installation

1. **Clone the Repository**
```bash
git clone <repository-url>
cd HealthCare
```

2. **Restore Dependencies**
```bash
dotnet restore
```

3. **Configure Database Connection**

Update `appsettings.Development.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HealthCareDb;Trusted_Connection=true;TrustServerCertificate=true;"
  }
}
```

4. **Apply Migrations**
```bash
dotnet ef database update --project HealthCare.csproj
```

This will:
- Create the database
- Create all tables with relationships
- Seed initial data (Departments, AppointmentTypes, Patients, Doctors, etc.)

5. **Run the Application**
```bash
dotnet run
```

Application runs on: `https://localhost:5001` or `http://localhost:5000`

6. **Access Swagger UI**
Navigate to: `https://localhost:5001/swagger`

## 🔌 API Endpoints

### Base URL
```
https://localhost:5001/api
```

### Department Endpoints
```
GET    /api/departments              - Get all departments
GET    /api/departments/{id}         - Get department by ID
POST   /api/departments              - Create new department
PUT    /api/departments/{id}         - Update department
DELETE /api/departments/{id}         - Delete department
```

### Patient Endpoints
```
GET    /api/patients                 - Get all patients
GET    /api/patients/{id}            - Get patient by ID
POST   /api/patients                 - Create new patient
PUT    /api/patients/{id}            - Update patient
DELETE /api/patients/{id}            - Delete patient
```

### PatientDetails Endpoints
```
GET    /api/patientdetails           - Get all patient details
GET    /api/patientdetails/{id}      - Get patient details by ID
POST   /api/patientdetails           - Create patient details
PUT    /api/patientdetails/{id}      - Update patient details
DELETE /api/patientdetails/{id}      - Delete patient details
```

### Doctor Endpoints
```
GET    /api/doctors                  - Get all doctors
GET    /api/doctors/{id}             - Get doctor by ID
POST   /api/doctors                  - Create new doctor
PUT    /api/doctors/{id}             - Update doctor
DELETE /api/doctors/{id}             - Delete doctor
```

### DoctorDetails Endpoints
```
GET    /api/doctordetails            - Get all doctor details
GET    /api/doctordetails/{id}       - Get doctor details by ID
POST   /api/doctordetails            - Create doctor details
PUT    /api/doctordetails/{id}       - Update doctor details
DELETE /api/doctordetails/{id}       - Delete doctor details
```

### AppointmentType Endpoints
```
GET    /api/appointmenttypes         - Get all appointment types
GET    /api/appointmenttypes/{id}    - Get appointment type by ID
POST   /api/appointmenttypes         - Create appointment type
PUT    /api/appointmenttypes/{id}    - Update appointment type
DELETE /api/appointmenttypes/{id}    - Delete appointment type
```

### Appointment Endpoints
```
GET    /api/appointments             - Get all appointments
GET    /api/appointments/{id}        - Get appointment by ID
POST   /api/appointments             - Create appointment
PUT    /api/appointments/{id}        - Update appointment
DELETE /api/appointments/{id}        - Delete appointment
```

### Response Format

All endpoints return a standardized JSON response:

**Success Response (200, 201)**
```json
{
  "success": true,
  "message": "Operation successful",
  "statusCode": 200,
  "data": {
    "id": 1,
    "firstName": "John",
    ...
  }
}
```

**Error Response (400, 404, 500)**
```json
{
  "success": false,
  "message": "Error description",
  "statusCode": 400,
  "data": null
}
```

## ⚙️ Configuration

### appsettings.Development.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=HealthCareDb;Trusted_Connection=true;"
  },
  "AllowedHosts": "*",
  "Jwt": {
    "SecretKey": "your-development-secret",
    "Issuer": "HealthCareAPI",
    "Audience": "HealthCareClient",
    "ExpirationMinutes": 60
  }
}
```

### appsettings.Production.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_PRODUCTION_SERVER;Database=HealthCareDb;User Id=sa;Password=your-password;"
  },
  "AllowedHosts": "your-domain.com"
}
```

## 🧪 Running Tests

### Run All Tests
```bash
dotnet test
```

### Run Specific Test Class
```bash
dotnet test --filter FullyQualifiedName~DepartmentServiceTests
```

### Run with Coverage (install coverlet first)
```bash
dotnet add package coverlet.collector
dotnet test /p:CollectCoverage=true
```

### Test Framework Details
- **Framework**: xUnit
- **Mocking**: Moq
- **Assertions**: FluentAssertions

### Test Structure
Each service has a dedicated test class:
- `DepartmentServiceTests` - Department business logic
- `PatientServiceTests` - Patient management
- `AppointmentServiceTests` - Appointment scheduling

Tests focus on:
- ✅ Getting all entities
- ✅ Getting single entity by ID
- ✅ Creating new entities
- ✅ Updating entities
- ✅ Deleting entities
- ✅ Error handling

## ✨ Features

### 1. **Clean Architecture**
- Separation of concerns across layers
- Dependency Injection for loose coupling
- Easy to test and maintain

### 2. **Data Validation**
- Server-side validation using FluentValidation
- Request parameter validation
- Email, phone number, and date validation
- Custom validation rules

### 3. **Audit Trail**
- All entities track CreatedBy, CreatedOn
- Track modifications with ModifiedBy, ModifiedOn
- Automatic timestamp management

### 4. **Async/Await Programming**
- Fully asynchronous operations
- Non-blocking database calls
- Better scalability and performance

### 5. **Standard HTTP Status Codes**
- 200 OK - Successful GET/PUT
- 201 Created - Successful POST
- 400 Bad Request - Validation errors
- 404 Not Found - Resource not found
- 500 Internal Server Error - Server errors

### 6. **Database First Approach**
- Code-first migrations with EF Core
- Automatic database creation
- Seed data for testing

### 7. **Comprehensive Logging**
- ILogger integration
- Error and information logging
- Request/response tracking

### 8. **CORS Support**
- Cross-Origin Resource Sharing enabled
- Configurable in appsettings

### 9. **API Documentation**
- Swagger/OpenAPI integration
- Auto-generated API documentation
- Interactive API testing via Swagger UI

## 📝 Sample API Calls

### Create a Patient
```bash
POST /api/patients
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john.doe@example.com",
  "phoneNumber": "555-0101",
  "dateOfBirth": "1980-05-15",
  "gender": "Male"
}
```

### Create an Appointment
```bash
POST /api/appointments
Content-Type: application/json

{
  "patientId": 1,
  "doctorId": 1,
  "appointmentTypeId": 1,
  "appointmentDate": "2024-03-20",
  "appointmentTime": "2024-03-20T10:00:00Z",
  "location": "Medical Center, Building A, Room 302",
  "notes": "Annual checkup"
}
```

### Update Appointment Status
```bash
PUT /api/appointments/1
Content-Type: application/json

{
  "patientId": 1,
  "doctorId": 1,
  "appointmentTypeId": 1,
  "appointmentDate": "2024-03-20",
  "appointmentTime": "2024-03-20T10:00:00Z",
  "status": "Confirmed",
  "location": "Medical Center, Building A, Room 302",
  "notes": "Annual checkup"
}
```

## 🔐 Security Considerations

1. **Input Validation**: All inputs validated server-side
2. **SQL Injection**: Protected via EF Core parameterized queries
3. **Environment Variables**: Use for sensitive configuration
4. **JWT Support**: Framework ready for JWT authentication
5. **HTTPS**: Enforced in production

## 📚 Additional Resources

- [Microsoft .NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core)
- [Clean Architecture Guide](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure)

## 📄 License

This project is licensed under the MIT License - see LICENSE file for details.

## 👥 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

**Last Updated**: March 2024
**Version**: 1.0.0
**Status**: Production Ready ✅
