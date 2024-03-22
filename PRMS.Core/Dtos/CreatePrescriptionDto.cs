using System.ComponentModel.DataAnnotations;

namespace PRMS.Core.Dtos;

public record CreatePrescriptionDto
{
    [Required] public string Name { get; init; }
    [Required] public string Dosage { get; init; }
    [Required] public string Frequency { get; init; }
    [Required] public string Diagnosis { get; init; }
    public string? Duration { get; init; }
    public string? Instruction { get; init; }
}