using System.ComponentModel.DataAnnotations;

namespace PRMS.Core.Dtos;

public class AdminRegisterDTO
{
    [Required] public string FirstName { get; init; }
    public string? MiddleName { get; init; }
    [Required] public string LastName { get; init; }
    [Required] public string PhoneNumber { get; init; }
    [Required] public string Street { get; init; }
    [Required] public string City { get; init; }
    [Required] public string State { get; init; }
    [Required] public string Country { get; init; }
    [Required] [EmailAddress] public string Email { get; init; }

    [Required] public string Password { get; init; }
}