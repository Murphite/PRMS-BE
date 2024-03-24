using PRMS.Domain.Enums;

namespace PRMS.Domain.Entities;

public class Medication : Entity, IAuditable
{
    public string PatientId { get; set; }
    public string? PrescriptionId { get; set; }
    public string Name { get; set; }
    public string Dosage { get; set; }
    public string Frequency { get; set; }
    public string? Duration { get; set; }
    public string? Instruction { get; set; }
    public MedicationStatus MedicationStatus { get; set; }

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public Patient Patient { get; set; }
    public Prescription? Prescription { get; set; }
}