namespace PRMS.Core.Dtos;

public class PatientDetailsDto
{
    public string FullName { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public string? Image { get; init; }
    public string Dob { get; init; }
    public string Gender { get; init; }
    public string BloodGroup { get; init; }
    public float Height { get; init; }
    public float Weight { get; init; }
    public string? PrimaryCarePhysican { get; init; }
    public string CurrentMedications { get; set; }
    public string Allergies { get; set; }
    public string MedicalConditions { get; set; }
    public string PhysicianName { get; set; }
    public string Location { get; set; }
    public string AppointmentStartTime { get; set; }
    public string AppointmentEndTime { get; set; }
    public int NoOfVisits { get; set; }
}