namespace PRMS.Domain.Entities;

public class Prescription : Entity, IAuditable
{
    public string PatientId { get; set; }
    public string PhysicianId { get; set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    
    public Patient Patient { get; set; }
    public Physician Physician { get; set; }
    public ICollection<Medication> Medications { get; set; }
}