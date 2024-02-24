using Microsoft.AspNetCore.Identity;

namespace PRMS.Domain.Entities;

public class User : IdentityUser, IAuditable
{
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string? ImageUrl { get; set; }
    public string? PublicId { get; set; }
    public string? AddressId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    
    public Address? Address { get; set; }
}