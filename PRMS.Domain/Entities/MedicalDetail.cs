using PRMS.Domain.Enums;

namespace PRMS.Domain.Entities;

public class MedicalDetail : Entity, IAuditable
{
    public string PatientId { get; set; }
    public MedicalDetailsType MedicalDetailsType { get; set; }
    public string Value { get; set; }
    
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}