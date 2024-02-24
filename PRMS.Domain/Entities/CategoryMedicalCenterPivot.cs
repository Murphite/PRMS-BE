namespace PRMS.Domain.Entities;

public class CategoryMedicalCenterPivot : Entity
{
    public string MedicalCenterCategoryId { get; set; }
    public MedicalCenterCategory MedicalCenterCategory { get; set; }

    public string MedicalCenterId { get; set; }
    public MedicalCenter MedicalCenter { get; set; }
}