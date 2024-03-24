using PRMS.Domain.Enums;

namespace PRMS.Core.Dtos;

    public class GetPhysicianAppointmentsByDateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageUrl { get; set; }
        public string Email { get; set; }
        public float Height { get; set; }
        public float Weight { get; set; }
        public BloodGroup BloodType { get; set; }
        public string PhysicianName { get; set; }
        public List<PatientMedication> CurrentMedication { get; set; } 
        public List<MedicalDetailsType> MedicalConditions { get; set; } 
        public List<MedicalDetailsType> Allergies { get; set; } 
        public DateTimeOffset Date { get; set; }

    }