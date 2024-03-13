using PRMS.Core.Dtos;
using PRMS.Domain.Enums;

namespace PRMS.Core.Abstractions;

public interface IAdminPatientService
{
    Task<Result> UpdateFromAdminAsync(UpdatePatientFromAdminDto dto, string userId);
    Task<Result> CreatePatient(CreatePatientFromAdminDto patientDto, string userId);
    Task<Result> UpdateAdminAppointmentStatus(string userId, AppointmentStatus status);
}