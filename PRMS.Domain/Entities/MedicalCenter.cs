using PRMS.Domain.Enums;

namespace PRMS.Domain.Entities;

public class MedicalCenter : Entity, IAuditable
{
    public MedicalCenterType Type { get; set; }
    public string Name { get; set; }
    public string AddressId { get; set; }
    public string? ImageUrl { get; set; }
    public string? PublicId { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public Address Address { get; set; }
    public ICollection<Patient> Patients { get; set; }
    public ICollection<Physician> Physicians { get; set; }
    public ICollection<MedicalCenterReview> Reviews { get; set; }
    public ICollection<MedicalCenterCategory> MedicalCenterCategories { get; set; }
}