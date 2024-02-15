namespace PRMS.Domain.Entities;

public class Clinic : Entity, IAuditable
{
    public string Name { get; set; }
    public string AddressId { get; set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    
    public Address Address { get; set; }
    public ICollection<User> Users { get; set; }
}