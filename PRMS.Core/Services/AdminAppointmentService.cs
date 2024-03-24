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

    public async Task<Result> GetAllPhysicianRangedAppointments(string physicianUserId)
    {
        var physician = await _userManager.FindByIdAsync(physicianUserId);
        if (physician == null)
        {
            return new Error[] { new("User.Error", "This physician is not registered") };
        }
        DateTime currentDate = DateTime.Today;

        DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1); //first day of the current month             

        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1); //last day of the current month
        var appointments = _repository.GetAll<Appointment>()
            .Where(c => c.PhysicianId == physicianUserId && c.Date >= firstDayOfMonth && c.Date <= lastDayOfMonth)
            .Include(c => c.Patient)
            .OrderByDescending(c => c.Date)
            .Select(r => new PhysicianRangedAppointmentDto
            {
                FirstName = r.Patient.User.FirstName,
                LastName = r.Patient.User.LastName,
                ImageUrl = r.Patient.User.ImageUrl,
                Date = r.Date
            });
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

    public async Task<Result<PaginatorDto<IEnumerable<MonthlyAppointmentsDto>>>> GetMonthlyAppointmentsForYear(string physicianUserId,string status, int year, PaginationFilter paginationFilter)
    {

        var physician = await _userManager.FindByIdAsync(physicianUserId);
        if (physician == null)
        {
            return new Error[] { new("User.Error", "This physician is not registered") };
        }

        // Parse the status string to the AppointmentStatus enum
            if (!Enum.TryParse(status, out AppointmentStatus appointmentStatus))
        {
            return new Error[] { new Error("Status.Error", "Invalid status value") };
        }

        var monthlyAppointments = await _repository.GetAll<Appointment>()
        .Where(a => a.Date.Year == year && a.Status == appointmentStatus)
        .Include(a => a.Patient.User)
        .Select(a => new
        {
            a.Date.Month,
            a.Date,
            PatientName = $"{a.Patient.User.FirstName} {a.Patient.User.LastName}",
            a.Status,
            a.Patient.User.ImageUrl
        })
        .AsQueryable()
        .GroupBy(a => a.Month)
        .Select(g => new MonthlyAppointmentsDto
        {
            Month = g.Key,
            Date = g.FirstOrDefault().Date,
            PatientName = g.FirstOrDefault().PatientName,
            Status = g.FirstOrDefault().Status,
            ImageUrl = g.FirstOrDefault().ImageUrl
        }).Paginate(paginationFilter);

        return monthlyAppointments;
    }
}