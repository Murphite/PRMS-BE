namespace PRMS.Domain.Entities;

public class Physician : Entity, IAuditable
{
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Speciality { get; set; }
    public string MedicalCenterId { get; set; }
    public string About { get; set; }
    public string WorkingTime { get; set; }
    public int YearsOfExperience { get; set; }
    
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    
    public User User { get; set; }
    public MedicalCenter MedicalCenter { get; set; }
    public ICollection<PhysicianReview> Reviews { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
}