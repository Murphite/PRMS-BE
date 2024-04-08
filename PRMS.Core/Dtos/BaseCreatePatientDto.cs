using PRMS.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PRMS.Core.Dtos;

public abstract class BaseCreatePatientDto
{
    [Required] public string FirstName { get; set; }
    [Required] public string LastName { get; set; }
    public string? MiddleName { get; set; }
    [Required] public DateOnly DateOfBirth { get; set; }
    [Required] public Gender Gender { get; set; }
    [Required] public string PhoneNumber { get; set; }
    public UpdatePatientAddressDto? Address { get; set; }
    public ICollection<MedicalDetailsDto> MedicalDetails { get; set; }
    [Required] public float Height { get; set; }
    [Required] public float Weight { get; set; }
    [Required] public BloodGroup BloodGroup { get; set; }
    public string? PrimaryPhysicianName { get; set; }
    public string? PrimaryPhysicianEmail { get; set; }
    public string? PrimaryPhysicianPhoneNo { get; set; }
    public ICollection<AddPatientMedicationDto> Medications { get; set; }
    [Required] public string EmergencyContactName { get; set; }
    [Required] public string EmergencyContactRelationship { get; set; }
    [Required] public string EmergencyContactPhoneNo { get; set; }
}