using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Utilities;
using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Core.Services;

public class AdminPatientService : IAdminPatientService
{
    private readonly IRepository _repository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public AdminPatientService(IRepository repository, UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PatientDetailsDto>> GetPatientDetails(string patientUserId)
    {
        var user = await _userManager.FindByIdAsync(patientUserId);
        
        if(user is null) 
            return new Error[] {new("User.Error", "User Not Found")};
        
        var patients = _repository.GetAll<Patient>()
            .Where(p => p.UserId == patientUserId);

        var patientDetails = await patients
            .Include(p => p.User)
            .Include(p => p.Medications)
            .Include(p => p.MedicalDetails)
            .Select(p => new PatientDetailsDto
            {
                FullName = $"{p.User.FirstName} {p.User.LastName}",
                Email = p.User.Email!,
                Phone = p.User.PhoneNumber,
                Image = p.User.ImageUrl,
                Dob = p.DateOfBirth.ToString(),
                Gender = p.Gender.ToString(),
                BloodGroup = p.BloodGroup.ToString(),
                Height = p.Height,
                Weight = p.Weight,
                PrimaryCarePhysican = p.PrimaryPhysicanName,
            })
            .FirstOrDefaultAsync();

        if (patientDetails is null)
            return new Error[] { new("Patient.Error", "Patient Not Found") };

        var latestAppointment = await _repository.GetAll<Appointment>()
            .Include(a => a.Physician)
            .ThenInclude(p => p.MedicalCenter)
            .Where(a => a.PatientId == patientUserId)
            .OrderByDescending(a => a.Date)
            .FirstOrDefaultAsync();

        if (latestAppointment is not null)
        {
            patientDetails.PhysicianName = latestAppointment.Physician.User.FirstName + " " +
                                           latestAppointment.Physician.User.LastName;
            patientDetails.Location = latestAppointment.Physician.MedicalCenter.Address.ToString();
            patientDetails.AppointmentStartTime = latestAppointment.Date.TimeOfDay.ToString();
            patientDetails.AppointmentEndTime = latestAppointment.Date.AddMinutes(30).TimeOfDay.ToString();
            patientDetails.NoOfVisits = await patients.Select(p => p.Appointments.Count).FirstOrDefaultAsync();
        }

        patientDetails.CurrentMedications = string.Join(", ", patients.First().Medications.Select(m => m.Name));
        patientDetails.Allergies = string.Join(", ",
            patients.First().MedicalDetails.Where(md => md.MedicalDetailsType == MedicalDetailsType.Allergy)
                .Select(md => md.Value));
        patientDetails.MedicalConditions = string.Join(", ",
            patients.First().MedicalDetails.Where(md => md.MedicalDetailsType == MedicalDetailsType.MedicalCondition)
                .Select(md => md.Value));

        return patientDetails;
    }

    public async Task<Result<PaginatorDto<IEnumerable<PatientDto>>>> GetListOfPatients(PaginationFilter paginationFilter)
    {
        var patients = await _repository.GetAll<Patient>()
            .Include(patient => patient.User)
            .Include(patient => patient.Appointments)
            .Where(patient => patient.Appointments.Any())
            .Select(patient => new PatientDto
            {
                UserId = patient.UserId,
                PatientId = patient.Id,
                PatientName = $"{patient.User.FirstName} {patient.User.LastName}",
                ImageUrl = patient.User.ImageUrl,
                Email = patient.User.Email!,
                StartTime = patient.Appointments.OrderByDescending(c => c.CreatedAt).First().Date.ToString("hh:mm tt"),
                EndTime = patient.Appointments.OrderByDescending(c => c.CreatedAt).First().Date.AddMinutes(30).ToString("hh:mm tt"),
                AppointmentDate = patient.CreatedAt.ToString("MMMM dd, yyyy"),
                NoOfAppointments = patient.Appointments.Count
            })
            .Paginate(paginationFilter);

        return patients;
    }

    public async Task<Result> UpdateFromAdminAsync(UpdatePatientFromAdminDto dto, string userId)
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

    public async Task<Result> CreatePatient(CreatePatientFromAdminDto patientDto, string userId)
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
        existingUser.ImageUrl = patientDto.ImageUrl ?? existingUser.ImageUrl;
        existingUser.Address.Street = patientDto.Address.Street ?? existingUser.Address.Street;
        existingUser.Address.City = patientDto.Address.City ?? existingUser.Address.City;
        existingUser.Address.State = patientDto.Address.State ?? existingUser.Address.State;
        existingUser.Address.Country = patientDto.Address.Country ?? existingUser.Address.Country;

        await _userManager.UpdateAsync(existingUser);

        var newPatient = new Patient
        {
            User = existingUser,
            DateOfBirth = patientDto.DateOfBirth,
            Gender = patientDto.Gender,
            BloodGroup = patientDto.BloodGroup,
            Medications = (ICollection<Medication>)patientDto.Medications,
            Height = patientDto.Height,
            Weight = patientDto.Weight,
            PrimaryPhysicanEmail = patientDto.PrimaryPhysicianEmail,
            PrimaryPhysicanName = patientDto.PrimaryPhysicianName,
            PrimaryPhysicanPhoneNo = patientDto.PrimaryPhysicianPhoneNo,
            MedicalDetails = (ICollection<MedicalDetail>)patientDto.MedicalDetails,
            EmergencyContactName = patientDto.EmergencyContactName,
            EmergencyContactPhoneNo = patientDto.EmergencyContactPhoneNo,
            EmergencyContactRelationship = patientDto.EmergencyContactRelationship,
            PhysicianId = patientDto.PhysicianId
        };

        await _repository.Add(newPatient);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result> UpdateAdminAppointmentStatus(string userId, AppointmentStatus status)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return new Error[] { new("User.Error", "User Not Found") };
        }

        var appointment = _repository.GetAll<Appointment>().FirstOrDefault(a => a.PatientId == userId);

        if (appointment == null)
        {
            return new Error[] { new("Appointment.Error", "No Appointment") };
        }

        appointment.Status = status;

        _repository.Update(appointment);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    public async Task<Result<Integer>> GetNewPatientsCount()
    {
        var endDate = DateTime.UtcNow;
        var startDate = new DateTime(endDate.Year, endDate.Month, 1);

        // Query the database for new patients within the past month
        var newPatientsCount = await _repository
            .GetAll<Patient>()
            .CountAsync(p => p.CreatedAt >= startDate && p.CreatedAt <= endDate);

        // Return the count of new patients
        var result = new Integer { Data = newPatientsCount };
        return Result.Success(result);
    }
}