using System.ComponentModel.DataAnnotations;

namespace PRMS.Core.Dtos;

public class CreateContactDto
{
    [Required] public string Name { get; set; }

    [Required] public string PhoneNumber { get; set; }

    [Required] public string EmailAddress { get; set; }

    [Required] public string StreetNumber { get; set; }

    [Required] public string StreetName { get; set; }

    [Required] public string City { get; set; }

    [Required] public string State { get; set; }

    [Required] public string Country { get; set; }
}