using PRMS.Domain.Enums;

namespace PRMS.Domain.Entities;

public class Appointment : Entity, IAuditable
{
    public string PatientId { get; set; }
    public string PhysicianId { get; set; }
    public DateTimeOffset Date { get; set; }
    public string? Reason { get; set; }
    public AppointmentStatus Status { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public Patient Patient { get; set; }
    public Physician Physician { get; set; }
}