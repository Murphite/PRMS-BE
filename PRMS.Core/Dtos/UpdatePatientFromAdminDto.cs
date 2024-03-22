using PRMS.Domain.Enums;

namespace PRMS.Core.Dtos;

public class UpdatePatientFromAdminDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MiddleName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public Gender? Gender { get; set; }
    public BloodGroup? BloodGroup { get; set; }
    public float? Height { get; set; }
    public float? Weight { get; set; }
    public string? PrimaryPhysicanName { get; set; }
    public string? PrimaryPhysicanEmail { get; set; }
    public string? PrimaryPhysicanPhoneNo { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhoneNo { get; set; }
    public string? EmergencyContactRelationship { get; set; }
}