using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface IAdminAppointmentService
{
    public Task<Result<PaginatorDto<IEnumerable<PhysicianPatientsAppointmentsDto>>>> GetPatientAppointments(
        string physicianUserId, string? status, PaginationFilter paginationFilter);

    public Task<Result<IEnumerable<PhysicianRangedAppointmentDto>>> GetCurrentMonthAppointment(
        string physicianUserId);

    public Task<Result<IEnumerable<MonthlyAppointmentsDto>>> GetMonthlyAppointmentCountForYear(string physicianUserId,
        string? status, int year);

    public Task<Result<PaginatorDto<IEnumerable<GetPhysicianAppointmentsByDateDto>>>>
        GetAllPhysicianAppointmentsSortedByDate(string physicianUserId, PaginationFilter paginationFilter);
}