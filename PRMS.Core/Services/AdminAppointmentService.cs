using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Utilities;
using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Core.Services;

public class AdminAppointmentService : IAdminAppointmentService
{
    private readonly UserManager<User> _userManager;
    private readonly IRepository _repository;

    public AdminAppointmentService(UserManager<User> userManager, IRepository repository)
    {
        _userManager = userManager;
        _repository = repository;
    }

    public async Task<Result> GetAllPhysicianRangedAppointments(string physicianUserId, PaginationFilter paginationFilter, DateTimeOffset startDate, DateTimeOffset endDate)
    {
        var physician = await _userManager.FindByIdAsync(physicianUserId);
        if (physician == null)
        {
            return new Error[] { new("User.Error", "This physician is not registered") };
        }
        var appointments = await _repository.GetAll<Appointment>()
            .Where(c => c.PhysicianId == physicianUserId && c.Date >= startDate && c.Date <= endDate)
            .Include(c => c.Patient)
            .OrderByDescending(c => c.Date)
            .Select(r => new PhysicianRangedAppointmentDto
            {
                FirstName = r.Patient.User.FirstName,
                LastName = r.Patient.User.LastName,
                ImageUrl = r.Patient.User.ImageUrl,
                Date = r.Date
            }).Paginate(paginationFilter);
        return Result.Success(appointments);
    }

    public async Task<Result> GetPatientAppointments(string physicianUserId, string? status,
        PaginationFilter paginationFilter)
    {
        var physicianUser = await _userManager.FindByIdAsync(physicianUserId);

        if (physicianUser == null)
        {
            return new Error[] { new("User.Error", "User Not Found") };
        }

        var physicianId = await _repository.GetAll<Physician>()
            .Where(x => x.UserId == physicianUser.Id)
            .Select(x => x.Id)
            .FirstAsync();

        var physicianAppointments = _repository.GetAll<Appointment>()
            .Where(p => p.PhysicianId == physicianId && p.Date >= DateTime.Now && p.Status == AppointmentStatus.Pending)
            .OrderByDescending(p => p.Date)
            .Include(p => p.Patient.MedicalDetails)
            .Include(p => p.Patient.Medications)
            .Include(p => p.Patient.Prescriptions)
            .Include(p => p.Patient.User);

        var appointmentToReturn = physicianAppointments.Select(p => new PhysicianPatientsAppointmentsDto(
            $"{p.Patient.User.FirstName} {p.Patient.User.LastName}",
            p.Patient.User.Email,
            p.Patient.User.ImageUrl,
            p.Date,
            p.Patient.BloodGroup.ToString(),
            p.Patient.Height,
            p.Patient.Weight,
            p.Patient.PrimaryPhysicanName,
            p.Patient.Prescriptions.Select(x => x.Diagnosis).ToList(),
            p.Patient.Medications.Select(m => new PatientMedication(
                m.Dosage,
                m.Name,
                m.Frequency
            )).ToList()
        )).Paginate(paginationFilter);

        return Result.Success(appointmentToReturn);
    }
}