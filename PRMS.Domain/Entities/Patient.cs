using PRMS.Domain.Enums;

namespace PRMS.Domain.Entities;

public class Patient: Entity, IAuditable
{
    public string UserId { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public float Height { get; set; }
    public float Weight { get; set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    
    public User User { get; set; }
    public ICollection<Medication> Medications { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; }
    public ICollection<MedicalDetail> MedicalDetails { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Favorite> Favorites { get; set; }
}