using PRMS.Domain.Enums;

namespace PRMS.Domain.Entities;

public class Patient: Entity, IAuditable
{
    public string UserId { get; set; }
    public string? PhysicianId { get; set; }
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
    public DateTimeOffset CreatedAt { get; set; } 
    public DateTimeOffset UpdatedAt { get; set; }
    
    public User User { get; set; }
    public Physician? Physician { get; set; }
    public ICollection<Medication> Medications { get; set; }  = new List<Medication>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<MedicalDetail> MedicalDetails { get; set; } = new  List<MedicalDetail>();
    public ICollection<Appointment> Appointments { get; set; } = new  List<Appointment>();
    public ICollection<Favorite> Favorites { get; set; } = new  List<Favorite>();
}