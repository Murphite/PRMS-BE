using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface IAppointmentService
{
    public Task<Result<IEnumerable<FetchPhysicianAppointmentsUserDto>>> GetAppointmentsForPhysician(string physicianUserId, DateTimeOffset startDate, DateTimeOffset endDate);
}