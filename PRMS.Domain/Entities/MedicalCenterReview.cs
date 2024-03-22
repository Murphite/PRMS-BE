namespace PRMS.Domain.Entities;
    
public class MedicalCenterReview : Entity, IAuditable
{
    public string MedicalCenterId { get; set; }
    public string PatientId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int Rating { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public MedicalCenter MedicalCenter { get; set; }
    public Patient Patient { get; set; }
}