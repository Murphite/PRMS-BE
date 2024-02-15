using System.ComponentModel.DataAnnotations;

namespace PRMS.Core.Dtos;

public record LoginUserDto
{
    [Required] public string Email { get; set; }

    [Required] public string Password { get; set; }
}