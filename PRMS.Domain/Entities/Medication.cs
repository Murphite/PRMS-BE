namespace PRMS.Domain.Entities;

public class Medication : Entity, IAuditable
{
    public string PatientId { get; set; }
    public string Name { get; set; }
    public string Dosage { get; set; }
    public string Frequency { get; set; }
    public string Duration { get; set; }
    public string? Instruction { get; set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    
    public Patient Patient { get; set; }
}