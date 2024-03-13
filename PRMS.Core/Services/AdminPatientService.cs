using Microsoft.AspNetCore.Identity;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

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
            PrimaryPhysicanEmail = patientDto.PrimaryPhysicanEmail,
            PrimaryPhysicanName = patientDto.PrimaryPhysicanName,
            PrimaryPhysicanPhoneNo = patientDto.PrimaryPhysicanPhoneNo,
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
}