using System.ComponentModel.DataAnnotations;

namespace PRMS.Core.Dtos;

public class AddPatientMedicationDto
{
    [Required] public string Name { get; set; } = string.Empty;
    [Required] public string Dosage { get; set; } = string.Empty;
    [Required] public string Frequency { get; set; } = string.Empty;
}