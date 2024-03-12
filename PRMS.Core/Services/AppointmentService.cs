using Microsoft.AspNetCore.Identity;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;
using System;
using System.Collections.Generic;
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

        public async Task<Result> GetAppointmentsForPhysician(string physicianId, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            var physician = await _userManager.FindByIdAsync(physicianId);
            if (physician == null)
                return new Error[] { new("Auth.Error", "user not found") };
            var physicianAppointments = _repository.GetAll<Appointment>()
                .Where(a => a.PhysicianId == physician.Id && a.Date >= startDate && a.Date <= endDate)
                .Select(c => new FetchPhysicianAppointmentsUserDto(c.Date)).ToList();
            return Result.Success();
        }
    }
}