using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions
{
    public interface  IAdminPatientService
    {
        Task<Result> UpdateFromAdminAsync(UpdatePatientFromAdminDto dto, string userId);
        Task<Result> CreatePatient(CreatePatientFromAdminDto patientDto, string userId);
    }
}
