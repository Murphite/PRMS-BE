using PRMS.Core.Dtos;
using PRMS.Domain.Enums;

namespace PRMS.Core.Abstractions;

public interface IAppointmentService
{
    public Task<Result<IEnumerable<FetchPhysicianAppointmentsUserDto>>> GetAppointmentsForPhysician(string physicianUserId, DateTimeOffset startDate, DateTimeOffset endDate);
    public Task<Result> UpdateAppointmentStatus(string userId, string appointmentId, AppointmentStatus status);
    public Task<Result> CreateAppointment(string userId, CreateAppointmentDto appointmentDto);
    public Task<Result> RescheduleAppointment(string appointmentId, RescheduleAppointmentDto rescheduleDto);
    public Task<Result<Integer>> GetTotalAppointmentsForDay(string physicianId, DateTimeOffset date);
}

//public class Integer
//{
//    public int Data { get; set;}
//}