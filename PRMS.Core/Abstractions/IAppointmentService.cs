using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface IAppointmentService
{
    public Task<Result> GetAppointmentsForPhysician(string physicianId, DateTimeOffset startDate, DateTimeOffset endDate);
}