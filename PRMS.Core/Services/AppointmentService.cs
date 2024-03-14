using Microsoft.AspNetCore.Identity;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PRMS.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IRepository _repository;
        private readonly UserManager<User> _userManager;

        public AppointmentService(IRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
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
    }
}