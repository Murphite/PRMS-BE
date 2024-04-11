using PRMS.Domain.Enums;

namespace PRMS.Core.Dtos;

public class MedicalDetailsDto
{
    public MedicalDetailsType MedicalDetailsType { get; set; }
    public string Value { get; set; }
}