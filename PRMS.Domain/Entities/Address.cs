namespace PRMS.Domain.Entities;

public class Address : Entity, IAuditable
{
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public override string ToString()
    {
        return $"{Street} {City} {State} {Country}";
    }
}