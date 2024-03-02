using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions
{
    public interface IPatientService
    {
        Task<Result> UpdateFromPatientAsync(UpdatePatientFromPatientDto dto, string userId);
        Task<Result> UpdateFromDoctorAsync(UpdatePatientFromDoctorDto dto, string patientId);
    }
}