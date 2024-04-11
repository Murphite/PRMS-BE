using PRMS.Domain.Enums;

namespace PRMS.Core.Dtos;

public class GetPhysicianAppointmentsByDateDto
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ImageUrl { get; set; }
    public string Email { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public BloodGroup BloodType { get; set; }
    public string? PhysicianName { get; set; }
    public IEnumerable<PatientMedication> CurrentMedication { get; set; }
    public IEnumerable<MedicalDetailsType> MedicalConditions { get; set; }
    public IEnumerable<MedicalDetailsType> Allergies { get; set; }
    public string Status { get; set; }
    public DateTimeOffset Date { get; set; }
    public string ScheduledDate { get; set; }
    public string TimeSlot { get; set; }
}