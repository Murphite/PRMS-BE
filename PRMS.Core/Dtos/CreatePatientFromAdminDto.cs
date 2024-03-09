using PRMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PRMS.Core.Dtos;

public class CreatePatientFromAdminDto : BaseCreatePatientDto
{
	public string? ImageUrl { get; set; }

	public string? PhysicianId { get; set; }

}
