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

        public async Task<Result> GetPatientAppointments(string userId, string status, PaginationFilter paginationFilter)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new Error[] { new("User.Error", "User Not Found") };
            }

            var physician = _repository.GetAll<Physician>().Where(x => x.UserId == user.Id).FirstOrDefault();

            var physicianAppointments = _repository.GetAll<Appointment>()
                .Where(p => p.PhysicianId == physician.Id && p.Date >= DateTime.Now && p.Status == AppointmentStatus.Pending)
                .OrderByDescending(p => p.Date)
                .Include(p => p.Patient)
                .ThenInclude(p => p.MedicalDetails)
                .Include(p => p.Patient)
                .ThenInclude(p => p.Medications)
                .Include(p => p.Patient)
                .ThenInclude(p => p.Prescriptions)
                .Include(p => p.Patient)
                .ThenInclude(p => p.User);

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

          if (physicianAppointments == null)
          {
            return new Error[] { new Error("Appointment.Error", "No Appointment") };
          }

        return Result.Success(appointmentToReturn);
        }
    }