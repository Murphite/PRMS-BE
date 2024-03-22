using System.ComponentModel.DataAnnotations;

namespace PRMS.Core.Dtos;

public class RegisterUserDto
{
    [Required] public string FirstName { get; init; }
    [Required] public string LastName { get; init; }
    [Required] public string PhoneNumber { get; init; }
    [Required] [EmailAddress] public string Email { get; init; }
    [Required] public string Password { get; init; }
    [Required] public string Street { get; set; }
    [Required] public string City { get; set; }
    [Required] public string State { get; set; }
    [Required] public string Country { get; set; }
    [Required] public double Latitude { get; set; }
    [Required] public double Longitude { get; set; }
}