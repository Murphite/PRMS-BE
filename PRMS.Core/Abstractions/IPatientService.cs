using PRMS.Core.Dtos;
using PRMS.Domain.Enums;

namespace PRMS.Core.Abstractions;

public interface IPatientService
{
    public interface IPatientService
    {
        public Task<Result> GetPatientAppointments(string userId, string? status, PaginationFilter paginationFilter);
    Task<Result> UpdateFromPatientAsync(UpdatePatientFromPatientDto dto, string userId);
    Task<Result> CreatePatient(string userId, CreatePatientFromUserDto patientDto);
    Task<Result> UpdateAppointmentStatus(string userId, AppointmentStatus status);
}