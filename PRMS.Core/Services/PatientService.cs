using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Utilities;
using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Core.Services;

public class PatientService : IPatientService
{
    private readonly IRepository _repository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IRepository repository, UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> UpdateFromPatientAsync(UpdatePatientFromPatientDto dto, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return new Error[] { new("User.Error", "User Not Found") };
        }

        var patient = _repository.GetAll<Patient>()
            .FirstOrDefault(x => x.UserId == user.Id);

        if (patient == null)
        {
            return new Error[] { new("Patients.Error", "Patient Not Found") };
        }

        patient.DateOfBirth = dto.DateOfBirth ?? patient.DateOfBirth;
        patient.Gender = dto.Gender ?? patient.Gender;
        patient.BloodGroup = dto.BloodGroup ?? patient.BloodGroup;
        patient.PrimaryPhysicanName = dto.PrimaryPhysicanName ?? patient.PrimaryPhysicanName;
        patient.PrimaryPhysicanEmail = dto.PrimaryPhysicanEmail ?? patient.PrimaryPhysicanEmail;
        patient.PrimaryPhysicanPhoneNo = dto.PrimaryPhysicanPhoneNo ?? patient.PrimaryPhysicanPhoneNo;
        patient.Height = dto.Height ?? patient.Height;
        patient.Weight = dto.Weight ?? patient.Weight;
        patient.EmergencyContactName = dto.EmergencyContactName ?? patient.EmergencyContactName;
        patient.EmergencyContactPhoneNo = dto.EmergencyContactPhoneNo ?? patient.EmergencyContactPhoneNo;
        patient.EmergencyContactRelationship = dto.EmergencyContactRelationship ?? patient.EmergencyContactRelationship;

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.MiddleName = dto.MiddleName ?? user.MiddleName;
        user.LastName = dto.LastName ?? user.LastName;
        user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;

        _repository.Update(patient);
        await _userManager.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<PaginatorDto<IEnumerable<PatientAppointmentsToReturnDTO>>>> GetPatientAppointments(string userId, string? status, PaginationFilter paginationFilter)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
            return new Error[] { new("User.Error", "User Not Found") };

        var patient = _repository.GetAll<Patient>().FirstOrDefault(x => x.UserId == user.Id);
        if (patient == null)
            return new Error[] { new("User.Error", "Patient Not Found") };

        IQueryable<Appointment> patientAppointments = _repository.GetAll<Appointment>()
            .Where(a => a.PatientId == patient.Id)
            .OrderByDescending(c => c.Date)
            .Include(p => p.Physician)
            .ThenInclude(u => u.User);

        if (!string.IsNullOrEmpty(status))
        {
            switch (status.ToLower())
            {
                case "upcoming":
                    patientAppointments = patientAppointments.Where(a => a.Status == AppointmentStatus.Pending);
                    break;
                case "completed":
                    patientAppointments = patientAppointments.Where(a => a.Status == AppointmentStatus.Completed);
                    break;
                case "cancelled":
                    patientAppointments = patientAppointments.Where(a => a.Status == AppointmentStatus.Cancelled);
                    break;
                default:

                    return new Error[] { new("Status.Error", "Invalid status entered") };
            }
        }

        var appointmentsToReturn = await patientAppointments
            .Select(p => new PatientAppointmentsToReturnDTO(
                $"{p.Physician.User.FirstName} {p.Physician.User.LastName}",
                p.Physician.Speciality,
                p.Physician.User.ImageUrl,
                p.Physician.MedicalCenter.Name,
                $"{p.Physician.MedicalCenter.Address.Street} {p.Physician.MedicalCenter.Address.City} {p.Physician.MedicalCenter.Address.State}",
                p.Date))
            .Paginate(paginationFilter);

        return appointmentsToReturn;
    }

    public async Task<Result> CreatePatient(string userId, CreatePatientFromUserDto patientDto)
    {
        var existingUser = await _userManager.FindByIdAsync(userId);
        if (existingUser is null)
        {
            return new Error[] { new("User.Error", "User Not Found") };
        }

        // Update the user data before creating a new Patient 
        existingUser.FirstName = patientDto.FirstName ?? existingUser.FirstName;
        existingUser.LastName = patientDto.LastName ?? existingUser.LastName;
        existingUser.MiddleName = patientDto.MiddleName ?? existingUser.MiddleName;
        existingUser.PhoneNumber = patientDto.PhoneNumber ?? existingUser.PhoneNumber;

        if (existingUser.Address == null && patientDto.Address != null)
        {
            existingUser.Address = new Address
            {
                Street = patientDto.Address.Street,
                City = patientDto.Address.City,
                State = patientDto.Address.State,
                Country = patientDto.Address.Country
            };
        }
        else
        {
            existingUser.Address.Street = patientDto.Address.Street;
            existingUser.Address.City = patientDto.Address.City;
            existingUser.Address.State = patientDto.Address.State;
            existingUser.Address.Country = patientDto.Address.Country;
        }

        await _userManager.UpdateAsync(existingUser);

        var newPatient = new Patient
        {
            User = existingUser,
            DateOfBirth = patientDto.DateOfBirth,
            Gender = patientDto.Gender,
            BloodGroup = patientDto.BloodGroup,
            Height = patientDto.Height,
            Weight = patientDto.Weight,
            PrimaryPhysicanEmail = patientDto.PrimaryPhysicianEmail,
            PrimaryPhysicanName = patientDto.PrimaryPhysicianName,
            PrimaryPhysicanPhoneNo = patientDto.PrimaryPhysicianPhoneNo,
            EmergencyContactName = patientDto.EmergencyContactName,
            EmergencyContactPhoneNo = patientDto.EmergencyContactPhoneNo,
            EmergencyContactRelationship = patientDto.EmergencyContactRelationship,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };

        newPatient.Medications = patientDto.Medications.Select(x => new Medication
        {
            PatientId = newPatient.Id,
            Name = x.Name,
            Dosage = x.Dosage,
            Frequency = x.Frequency,
        }).ToList();
        newPatient.MedicalDetails = patientDto.MedicalDetails.Select(x => new MedicalDetail
        {
            MedicalDetailsType = x.MedicalDetailsType,
            Value = x.Value
        }).ToList();

        await _repository.Add(newPatient);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }
}