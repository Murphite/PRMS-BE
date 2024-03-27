using PRMS.Core.Dtos;
using PRMS.Domain.Enums;

namespace PRMS.Core.Abstractions;

public interface IAdminPatientService
{
    Task<Result> UpdateFromAdminAsync(UpdatePatientFromAdminDto dto, string userId);
    Task<Result> CreatePatient(CreatePatientFromAdminDto patientDto, string userId);
    Task<Result> UpdateAdminAppointmentStatus(string userId, AppointmentStatus status);
    public Task<Result<PatientDetailsDto>> GetPatientDetails(string patientUserId);
    Task<Result<PaginatorDto<IEnumerable<PatientDto>>>>  GetListOfPatients(PaginationFilter paginationFilter);
    public Task<Result<Integer>> GetNewPatientsCount();
}

public class Integer
{
    public int Data { get; set; }
}