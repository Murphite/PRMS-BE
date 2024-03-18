using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface IAdminAppointmentService
{
    public Task<Result> GetPatientAppointments(string physicianUserId, string? status, PaginationFilter paginationFilter);
    public Task<Result> GetAllPhysicianRangedAppointments(string physicianUserId, PaginationFilter paginationFilter, DateTimeOffset startDate, DateTimeOffset endDate);

}