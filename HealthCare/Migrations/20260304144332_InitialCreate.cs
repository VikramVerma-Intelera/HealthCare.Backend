using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthCare.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppointmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DurationInMinutes = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppointmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    Speciality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doctors_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BloodType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmergencyContactName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmergencyContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllergiesDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalHistoryNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDetails_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    AppointmentTypeId = table.Column<int>(type: "int", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_AppointmentTypes_AppointmentTypeId",
                        column: x => x.AppointmentTypeId,
                        principalTable: "AppointmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DoctorDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    QualificationDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Experience = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ConsultationDuration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ClinicAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorDetails_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppointmentTypes",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Department", "Description", "DurationInMinutes", "IsActive", "ModifiedBy", "ModifiedOn", "Price", "TypeCode", "TypeName" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7906), "Primary Care", "Comprehensive health checkup and consultation with experienced physicians", 30, true, null, null, 75m, "GC", "General Consultation" },
                    { 2, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7908), "Specialist Care", "Heart health assessment with ECG and expert consultation", 45, true, null, null, 150m, "CC", "Cardiology Checkup" },
                    { 3, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7910), "Specialist Care", "Skin care consultation and treatment planning", 30, true, null, null, 100m, "DC", "Dermatology Consultation" },
                    { 4, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7911), "Virtual Care", "Online video consultation with healthcare professionals", 20, true, null, null, 50m, "TV", "Telemedicine Visit" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DepartmentCode", "DepartmentName", "Description", "IsActive", "ModifiedBy", "ModifiedOn" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7740), "PC", "Primary Care", "General healthcare services", true, null, null },
                    { 2, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7743), "SC", "Specialist Care", "Specialized healthcare services", true, null, null },
                    { 3, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7744), "VC", "Virtual Care", "Telemedicine and virtual consultations", true, null, null }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DateOfBirth", "Email", "FirstName", "Gender", "IsActive", "LastName", "ModifiedBy", "ModifiedOn", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7943), new DateTime(1980, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", "John", "Male", true, "Doe", null, null, "555-0101" },
                    { 2, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7946), new DateTime(1990, 8, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "jane.smith@example.com", "Jane", "Female", true, "Smith", null, null, "555-0102" },
                    { 3, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7948), new DateTime(1975, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "michael.j@example.com", "Michael", "Male", true, "Johnson", null, null, "555-0103" }
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "DepartmentId", "Email", "FirstName", "IsActive", "LastName", "LicenseNumber", "ModifiedBy", "ModifiedOn", "PhoneNumber", "Speciality" },
                values: new object[,]
                {
                    { 1, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8002), 2, "sarah.johnson@hospital.com", "Sarah", true, "Johnson", "MD-001", null, null, "555-1001", "Cardiology" },
                    { 2, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8004), 1, "michael.chen@hospital.com", "Michael", true, "Chen", "MD-002", null, null, "555-1002", "General Practice" },
                    { 3, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8006), 2, "emily.brown@hospital.com", "Emily", true, "Brown", "MD-003", null, null, "555-1003", "Dermatology" }
                });

            migrationBuilder.InsertData(
                table: "PatientDetails",
                columns: new[] { "Id", "Address", "AllergiesDescription", "BloodType", "City", "CreatedBy", "CreatedOn", "EmergencyContactName", "EmergencyContactPhone", "MedicalHistoryNotes", "ModifiedBy", "ModifiedOn", "PatientId", "PostalCode", "State" },
                values: new object[,]
                {
                    { 1, "123 Main St", null, "O+", "New York", "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7977), "Mary Doe", "555-0104", null, null, null, 1, "10001", "NY" },
                    { 2, "456 Oak Ave", null, "A+", "Boston", "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7979), "John Smith", "555-0105", null, null, null, 2, "02101", "MA" },
                    { 3, "789 Pine Rd", null, "B+", "Chicago", "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(7981), "Sarah Johnson", "555-0106", null, null, null, 3, "60601", "IL" }
                });

            migrationBuilder.InsertData(
                table: "Appointments",
                columns: new[] { "Id", "AppointmentDate", "AppointmentTime", "AppointmentTypeId", "CreatedBy", "CreatedOn", "DoctorId", "Location", "ModifiedBy", "ModifiedOn", "Notes", "PatientId", "Status" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 3, 9, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8055), new DateTime(2026, 3, 10, 0, 43, 32, 476, DateTimeKind.Utc).AddTicks(8061), 2, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8062), 1, "Medical Center, Building A, Room 302", null, null, "Annual checkup", 1, "Confirmed" },
                    { 2, new DateTime(2026, 3, 11, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8063), new DateTime(2026, 3, 12, 4, 43, 32, 476, DateTimeKind.Utc).AddTicks(8064), 1, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8065), 2, "Online Consultation", null, null, "Follow-up appointment", 2, "Scheduled" }
                });

            migrationBuilder.InsertData(
                table: "DoctorDetails",
                columns: new[] { "Id", "Bio", "ClinicAddress", "ConsultationDuration", "ConsultationFee", "CreatedBy", "CreatedOn", "DoctorId", "Experience", "ModifiedBy", "ModifiedOn", "QualificationDetails" },
                values: new object[,]
                {
                    { 1, null, null, "45 min", 150m, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8029), 1, "15 years", null, null, "MD, Board Certified in Cardiology" },
                    { 2, null, null, "30 min", 75m, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8030), 2, "10 years", null, null, "MD, General Practice" },
                    { 3, null, null, "30 min", 100m, "System", new DateTime(2026, 3, 4, 14, 43, 32, 476, DateTimeKind.Utc).AddTicks(8032), 3, "12 years", null, null, "MD, Board Certified in Dermatology" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentTypeId",
                table: "Appointments",
                column: "AppointmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId",
                table: "Appointments",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorDetails_DoctorId",
                table: "DoctorDetails",
                column: "DoctorId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDetails_PatientId",
                table: "PatientDetails",
                column: "PatientId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "DoctorDetails");

            migrationBuilder.DropTable(
                name: "PatientDetails");

            migrationBuilder.DropTable(
                name: "AppointmentTypes");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Departments");
        }
    }
}
