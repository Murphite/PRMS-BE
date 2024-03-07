using Microsoft.AspNetCore.Identity;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

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
}