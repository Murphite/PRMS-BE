using Microsoft.AspNetCore.Identity;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using PRMS.Domain.Enums;

namespace PRMS.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public AppointmentService(IRepository repository, IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<Result<IEnumerable<FetchPhysicianAppointmentsUserDto>>> GetAppointmentsForPhysician(
            string physicianUserId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var physicianId = _repository.GetAll<Physician>()
                .Where(x => x.UserId == physicianUserId)
                .Select(p => p.Id)
                .FirstOrDefault();

            if (physicianId == null)
                return new Error[] { new("Auth.Error", "user not found") };

            var physicianAppointments = await _repository.GetAll<Appointment>()
                .Where(a => a.PhysicianId == physicianId && a.Date >= startDate && a.Date <= endDate)
                .Select(c => new FetchPhysicianAppointmentsUserDto(c.Date))
                .ToListAsync();

            return physicianAppointments;
        }

        public async Task<Result> UpdateAppointmentStatus(string userId, string appointmentId, AppointmentStatus status)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new Error[] { new("User.Error", "User Not Found") };
            }

            var appointment = _repository.GetAll<Appointment>().FirstOrDefault(a => a.Id == appointmentId);

            if (appointment == null)
            {
                return new Error[] { new("Appointment.Error", "No Appointment") };
            }

            appointment.Status = status;

            _repository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> CreateAppointment(string userId, CreateAppointmentDto appointmentDto)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return new Error[] { new("User.NotFound", "User not found") };

            var patientId = _repository.GetAll<Patient>().First(p => p.UserId == user.Id).Id;
            var physicianId = _repository.GetAll<Physician>()
                .Where(p => p.UserId == appointmentDto.PhysicianUserId)
                .Select(p => p.Id)
                .First();
            
            var appointment = new Appointment
            {
                Date = appointmentDto.Date,
                Status = AppointmentStatus.Pending,
                PatientId = patientId,
                PhysicianId = physicianId,
                Reason = appointmentDto.Reason,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            await _repository.Add(appointment);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result> RescheduleAppointment(string appointmentId, RescheduleAppointmentDto rescheduleDto)
        {
            var appointment = _repository.GetAll<Appointment>().FirstOrDefault(a => a.Id == appointmentId);

            if (appointment == null)
            {
                return new Error[] { new("Appointment.Error", "No Appointment") };
            }

            appointment.Date = rescheduleDto.NewAppointmentDate;

            _repository.Update(appointment);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }

        public async Task<Result<Integer>> GetTotalAppointmentsForDay(string physicianId, DateTimeOffset date)
        {
            // Check if the physician exists
            var physicianExists = await _repository.GetAll<Physician>().AnyAsync(p => p.Id == physicianId);
            if (!physicianExists)
            {
                return new Error[] { new Error("PhysicianNotFound", "The physician does not exist.") };
            }

            // Query the total appointments for the day
            var totalAppointments = await _repository
                .GetAll<Appointment>()
                .CountAsync(a => a.PhysicianId == physicianId && a.CreatedAt.Date == date);

            var result = new Integer { Data = totalAppointments };
            return Result.Success(result);
        }
    }
}