namespace PRMS.Domain.Entities;

public class PhysicianReview : Entity, IAuditable
{
    public string PhysicianId { get; set; }
    public string PatientId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int Rating { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public Physician Physician { get; set; }
    public Patient Patient { get; set; }
}