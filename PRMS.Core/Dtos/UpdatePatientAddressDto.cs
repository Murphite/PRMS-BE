using System.ComponentModel.DataAnnotations;

namespace PRMS.Core.Dtos;

public class UpdatePatientAddressDto
{
    [Required] public string Street { get; set; }
    [Required] public string City { get; set; }
    [Required] public string State { get; set; }
    [Required] public string Country { get; set; }
    public double? Longitude { get; set; }
    public double? Latitude { get; set; }
}