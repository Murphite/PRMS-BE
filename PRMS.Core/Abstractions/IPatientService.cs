using PRMS.Core.Dtos;
using PRMS.Domain.Enums;

namespace PRMS.Core.Abstractions
{
    public interface IPatientService
    {
        Task<Result> UpdateFromPatientAsync(UpdatePatientFromPatientDto dto, string userId);
        Task<Result> CreatePatient(string userId, CreatePatientFromUserDto patientDto);
        Task<Result> UpdateAppointmentStatusAsync(string userId, AppointmentStatus status);
    }
}