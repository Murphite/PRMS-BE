using PRMS.Core.Dtos;
using PRMS.Domain.Enums;

namespace PRMS.Core.Abstractions;

public interface IPatientService
{
    public Task<Result<PaginatorDto<IEnumerable<PatientAppointmentsToReturnDTO>>>> GetPatientAppointments(string userId, string? status, PaginationFilter paginationFilter);
    Task<Result> UpdateFromPatientAsync(UpdatePatientFromPatientDto dto, string userId);
    Task<Result> CreatePatient(string userId, CreatePatientFromUserDto patientDto);
}