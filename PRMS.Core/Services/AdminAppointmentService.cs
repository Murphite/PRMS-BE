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

    public async Task<Result<IEnumerable<PhysicianRangedAppointmentDto>>> GetCurrentMonthAppointment(
        string physicianUserId)
    {
        var user = await _userManager.FindByIdAsync(physicianUserId);
        if (user == null)
        {
            return new Error[] { new("User.Error", "User not found") };
        }

        var physicianId = await _repository.GetAll<Physician>()
            .Where(p => p.UserId == physicianUserId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();

        if (physicianId is null)
        {
            return new Error[] { new("Physician.Error", "Physician not found") };
        }

        var currentDate = DateTime.Today;
        var firstDayOfMonth =
            new DateTime(currentDate.Year, currentDate.Month, 1); //first day of the current month             
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1); //last day of the current month

        var appointments = await _repository.GetAll<Appointment>()
            .Where(c => c.PhysicianId == physicianId && c.Date >= firstDayOfMonth && c.Date <= lastDayOfMonth)
            .Include(c => c.Patient)
            .Select(r => new PhysicianRangedAppointmentDto
            {
                FirstName = r.Patient.User.FirstName,
                LastName = r.Patient.User.LastName,
                ImageUrl = r.Patient.User.ImageUrl,
                Date = r.Date
            })
            .ToListAsync();

        return appointments;
    }

    public async Task<Result<PaginatorDto<IEnumerable<PhysicianPatientsAppointmentsDto>>>> GetPatientAppointments(
        string physicianUserId, string? status,
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
            .FirstOrDefaultAsync();

        if (physicianId is null)
        {
            return new Error[] { new("Physician.Error", "Physician not found") };
        }

        var physicianAppointments = _repository.GetAll<Appointment>()
            .Where(p => p.PhysicianId == physicianId && p.Date >= DateTimeOffset.UtcNow &&
                        p.Status == AppointmentStatus.Pending)
            .OrderByDescending(p => p.Date)
            .Include(p => p.Patient.Medications)
            .Include(p => p.Patient.Prescriptions)
            .Include(p => p.Patient.User);

        var appointmentToReturn = await physicianAppointments
            .Select(p => new PhysicianPatientsAppointmentsDto(
                p.Id,
                $"{p.Patient.User.FirstName} {p.Patient.User.LastName}",
                p.Patient.User.Email!,
                p.Patient.User.ImageUrl,
                p.Date.ToString("MMMM dd, yyyy"),
                p.Patient.BloodGroup.ToString(),
                p.Patient.Height,
                p.Patient.Weight,
                p.Patient.PrimaryPhysicanName,
                p.Patient.MedicalDetails,
                p.Patient.Prescriptions.Select(x => x.Diagnosis),
                p.Patient.Medications.Select(m => new PatientMedication(
                    m.Dosage,
                    m.Name,
                    m.Frequency
                )),
                p.Date.ToString("hh: mm tt"),
                p.Date.AddMinutes(30).ToString("hh:mm tt")
            )).Paginate(paginationFilter);

        return appointmentToReturn;
    }

    public async Task<Result<IEnumerable<MonthlyAppointmentsDto>>> GetMonthlyAppointmentCountForYear(
        string physicianUserId, string? status, int year)
    {
        var user = await _userManager.FindByIdAsync(physicianUserId);
        if (user == null)
        {
            return new Error[] { new("User.Error", "User not found") };
        }

        var physicianId = await _repository.GetAll<Physician>()
            .Where(x => x.UserId == user.Id)
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

        if (physicianId is null)
        {
            return new Error[] { new("Physician.Error", "Physician not found") };
        }

        var query = _repository.GetAll<Appointment>()
            .Where(a => a.PhysicianId == physicianId && a.Date.Year == year);

        if (Enum.TryParse(status, out AppointmentStatus appointmentStatus))
        {
            query = query.Where(a => a.Status == appointmentStatus);
        }

        var monthlyAppointments = await query
            .GroupBy(a => a.Date.Month)
            .Select(a => new MonthlyAppointmentsDto
            {
                Date = a.Key,
                AppointmentCount = a.Count(),
            })
            .ToListAsync();

        return monthlyAppointments;
    }

    public async Task<Result<PaginatorDto<IEnumerable<GetPhysicianAppointmentsByDateDto>>>>
        GetAllPhysicianAppointmentsSortedByDate(string physicianUserId, PaginationFilter paginationFilter)
    {
        var user = await _userManager.FindByIdAsync(physicianUserId);
        if (user == null)
        {
            return new Error[] { new("User.Error", "User not found") };
        }

        var physicianId = await _repository.GetAll<Physician>()
            .Where(p => p.UserId == physicianUserId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();

        if (physicianId is null)
        {
            return new Error[] { new("Physician.Error", "Physician not found") };
        }

        var appointments = await _repository.GetAll<Appointment>()
            .Where(c => c.PhysicianId == physicianId)
            .OrderByDescending(c => c.Date)
            .Select(r => new GetPhysicianAppointmentsByDateDto
            {
                Id = r.Id,
                FirstName = r.Patient.User.FirstName,
                LastName = r.Patient.User.LastName,
                Email = r.Patient.User.Email!,
                ImageUrl = r.Patient.User.ImageUrl,
                Height = r.Patient.Height,
                Weight = r.Patient.Weight,
                BloodType = r.Patient.BloodGroup,
                PhysicianName = r.Patient.PrimaryPhysicanName,
                Date = r.Date,
                ScheduledDate = r.Date.ToString("MMMM dd,yyyy"),
                TimeSlot = $"{r.Date:hh:mm tt} - {r.Date.AddMinutes(30):hh:mm tt}",
                Status = r.Status.ToString(),
                CurrentMedication = r.Patient.Medications
                    .Select(m => new PatientMedication
                    (
                        m.Dosage,
                        m.Name,
                        m.Frequency
                    )),
                MedicalConditions = r.Patient.MedicalDetails
                    .Where(md => md.MedicalDetailsType == MedicalDetailsType.MedicalCondition)
                    .Select(md => md.MedicalDetailsType),
                Allergies = r.Patient.MedicalDetails
                    .Where(md => md.MedicalDetailsType == MedicalDetailsType.Allergy)
                    .Select(md => md.MedicalDetailsType)
            })
            .Paginate(paginationFilter);

        return appointments;
    }
}