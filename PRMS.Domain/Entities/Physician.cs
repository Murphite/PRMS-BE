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

    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public User User { get; set; }
    public MedicalCenter MedicalCenter { get; set; }
    public ICollection<Patient> Patients { get; set; } = new List<Patient>();
    public ICollection<PhysicianReview> Reviews { get; set; } = new List<PhysicianReview>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}