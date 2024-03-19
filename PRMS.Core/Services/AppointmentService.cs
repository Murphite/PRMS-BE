using Microsoft.AspNetCore.Identity;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;
using System;
using System.Collections.Generic;
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
            var user = await _userManager.FindByIdAsync(physicianUserId);
            if (user == null)
                return new Error[] { new("Auth.Error", "user not found") };

            var physicianAppointments = await _repository.GetAll<Appointment>()
                .Include(a => a.Physician)
                .Where(a => a.PhysicianId == a.Physician.UserId && a.Date >= startDate && a.Date <= endDate)
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

            var date = appointmentDto.Date.ToDateTime(appointmentDto.Time);
            var appointment = new Appointment
            {
                Date = new DateTimeOffset(date).ToUniversalTime(),
                Status = AppointmentStatus.Pending,
                PatientId = patientId,
                PhysicianId = appointmentDto.PhysicianId,
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

    }
}