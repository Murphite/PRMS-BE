using PRMS.Core.Dtos;
using PRMS.Domain.Enums;

namespace PRMS.Core.Abstractions;

public interface IAppointmentService
{
    public Task<Result<IEnumerable<FetchPhysicianAppointmentsUserDto>>> GetAppointmentsForPhysician(string physicianUserId, DateTimeOffset startDate, DateTimeOffset endDate);
    public Task<Result> UpdateAppointmentStatus(string userId, string appointmentId, AppointmentStatus status);
}