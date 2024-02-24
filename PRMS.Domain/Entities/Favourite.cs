namespace PRMS.Domain.Entities;

public class Favorite : Entity
{
    public string PatientId { get; set; }
    public string EntityId { get; set; }
    public EntityType EntityType { get; set; }

    public Patient Patient { get; set; }
}

public enum EntityType
{
    MedicalCenter,
    Physician
}