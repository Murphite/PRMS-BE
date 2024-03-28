namespace PRMS.Domain.Entities;

public class MedicalCenterCategory : Entity
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }

    public ICollection<MedicalCenter> MedicalCenters { get; set; } = new List<MedicalCenter>();
}