using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Core.Dtos
{
    public class UpdatePatientFromDoctorDto
    {
        public string PatientId { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public BloodGroup BloodGroup { get; set; }
        public string? PrimaryPhysicanName { get; set; }
        public string? PrimaryPhysicanEmail { get; set; }
        public string? PrimaryPhysicanPhoneNo { get; set; }
        public string EmergencyContactName { get; set; }
        public string EmergencyContactRelationship { get; set; }
        public string EmergencyContactPhoneNo { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }
        public string? Dosage { get; set; }
        public string? Frequency { get; set; }
        public string? MedicationName { get; set; }
        public string? Value { get; set; }

        public Patient Patient { get; set; }
    }
}