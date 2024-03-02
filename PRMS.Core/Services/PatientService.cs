using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Data.Contexts;
using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Core.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository _repository;
        private readonly UserManager<User> _UserManager;

        public PatientService(IRepository repository, UserManager<User> UserManager)
        {
            _repository = repository;
            _UserManager = UserManager;
        }

        public async Task<Result> UpdateFromPatientAsync(UpdatePatientFromPatientDto dto, string userId)
        {
            try
            {
                var user = await _UserManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return new Error[] { new Error("User.Error", "User Not Found") };
                }

                // Find the patient by user id
                var patient = await _repository.FindById<Patient>(userId);

                if (patient == null)
                {
                    return new Error[] { new Error("Patients.Error", "Patient Not Found") };
                }

                // Update patient properties based on DTO
                patient.DateOfBirth = dto.DateOfBirth != null ? dto.DateOfBirth : patient.DateOfBirth;
                patient.Gender = dto.Gender != null ? dto.Gender : patient.Gender;
                patient.BloodGroup = dto.BloodGroup != null ? dto.BloodGroup : patient.BloodGroup;
                patient.PrimaryPhysicanName = dto.PrimaryPhysicanName ?? patient.PrimaryPhysicanName;
                patient.PrimaryPhysicanEmail = dto.PrimaryPhysicanEmail ?? patient.PrimaryPhysicanEmail;
                patient.PrimaryPhysicanPhoneNo = dto.PrimaryPhysicanPhoneNo ?? patient.PrimaryPhysicanPhoneNo;
                patient.Height = dto.Height != default(float) ? dto.Height : patient.Height;
                patient.Weight = dto.Weight != default(float) ? dto.Weight : patient.Weight;
                patient.EmergencyContactName = dto.EmergencyContactName ?? patient.EmergencyContactName;
                patient.EmergencyContactPhoneNo = dto.EmergencyContactPhoneNo ?? patient.EmergencyContactPhoneNo;
                patient.EmergencyContactRelationship = dto.EmergencyContactRelationship ?? patient.EmergencyContactRelationship;

                // Update user properties
                patient.User.FirstName = dto.FirstName ?? patient.User.FirstName;
                patient.User.MiddleName = dto.MiddleName ?? patient.User.MiddleName;
                patient.User.LastName = dto.LastName ?? patient.User.LastName;
                patient.User.Email = dto.Email ?? patient.User.Email;
                patient.User.PhoneNumber = dto.PhoneNumber ?? patient.User.PhoneNumber;
                patient.User.ImageUrl = dto.ImageUrl ?? patient.User.ImageUrl;

                // Update medications
                foreach (var medication in patient.Medications)
                {
                    medication.Dosage = dto.Dosage ?? medication.Dosage;
                    medication.Name = dto.MedicationName ?? medication.Name;
                    medication.Frequency = dto.Frequency ?? medication.Frequency;
                }

                // Update medical details
                foreach (var medicalDetail in patient.MedicalDetails)
                {
                    medicalDetail.Value = dto.Value ?? medicalDetail.Value;
                    // Update other medical detail properties as needed
                }



                // Save changes to the repository
                _repository.Update(patient);

                // Create and return a DTO representing the updated patient
                var updateDTO = new UpdatePatientFromPatientDto
                {
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    BloodGroup = patient.BloodGroup,
                    PrimaryPhysicanName = patient.PrimaryPhysicanName,
                    PrimaryPhysicanEmail = patient.PrimaryPhysicanEmail,
                    PrimaryPhysicanPhoneNo = patient.PrimaryPhysicanPhoneNo,
                    EmergencyContactName = patient.EmergencyContactName,
                    EmergencyContactPhoneNo = patient.EmergencyContactPhoneNo,
                    EmergencyContactRelationship = patient.EmergencyContactRelationship,
                    Height = patient.Height,
                    Weight = patient.Weight,
                    FirstName = patient.User.FirstName,
                    MiddleName = patient.User.MiddleName,
                    LastName = patient.User.LastName,
                    Email = patient.User.Email,
                    PhoneNumber = patient.User.PhoneNumber,
                    ImageUrl = patient.User.ImageUrl,
                    // Add other properties as needed
                };

                if (patient.Medications.Any())
                {
                    updateDTO.Dosage = patient.Medications.First().Dosage;
                    updateDTO.Frequency = patient.Medications.First().Frequency;
                    updateDTO.MedicationName = patient.Medications.First().Name;
                }

                if (patient.MedicalDetails.Any())
                {
                    updateDTO.Value = patient.MedicalDetails.First().Value;
                }

                return Result.Success(updateDTO);
            }
            catch (Exception ex)
            {
                return Result.Failure<UpdatePatientFromPatientDto>(new[] { new Error("Exception", ex.Message) });
            }
        }

        public async Task<Result> UpdateFromDoctorAsync(UpdatePatientFromDoctorDto dto, string patientId)
        {
            try
            {
                var user = await _UserManager.FindByIdAsync(patientId);

                if (user == null)
                {
                    return new Error[] { new Error("Patient.Error", "Patient Not Found") };
                }

                // Find the patient by user id
                var patient = await _repository.FindById<Patient>(patientId);

                if (patient == null)
                {
                    return new Error[] { new Error("Patients.Error", "Patient Not Found") };
                }

                // Update patient properties based on DTO
                patient.DateOfBirth = dto.DateOfBirth != null ? dto.DateOfBirth : patient.DateOfBirth;
                patient.Gender = dto.Gender != null ? dto.Gender : patient.Gender;
                patient.BloodGroup = dto.BloodGroup != null ? dto.BloodGroup : patient.BloodGroup;
                patient.PrimaryPhysicanName = dto.PrimaryPhysicanName ?? patient.PrimaryPhysicanName;
                patient.PrimaryPhysicanEmail = dto.PrimaryPhysicanEmail ?? patient.PrimaryPhysicanEmail;
                patient.PrimaryPhysicanPhoneNo = dto.PrimaryPhysicanPhoneNo ?? patient.PrimaryPhysicanPhoneNo;
                patient.Height = dto.Height != default(float) ? dto.Height : patient.Height;
                patient.Weight = dto.Weight != default(float) ? dto.Weight : patient.Weight;
                patient.EmergencyContactName = dto.EmergencyContactName ?? patient.EmergencyContactName;
                patient.EmergencyContactPhoneNo = dto.EmergencyContactPhoneNo ?? patient.EmergencyContactPhoneNo;
                patient.EmergencyContactRelationship = dto.EmergencyContactRelationship ?? patient.EmergencyContactRelationship;

                // Update user properties
                patient.User.FirstName = dto.FirstName ?? patient.User.FirstName;
                patient.User.MiddleName = dto.MiddleName ?? patient.User.MiddleName;
                patient.User.LastName = dto.LastName ?? patient.User.LastName;
                patient.User.Email = dto.Email ?? patient.User.Email;
                patient.User.PhoneNumber = dto.PhoneNumber ?? patient.User.PhoneNumber;
                patient.User.ImageUrl = dto.ImageUrl ?? patient.User.ImageUrl;

                // Update medications
                foreach (var medication in patient.Medications)
                {
                    medication.Dosage = dto.Dosage ?? medication.Dosage;
                    medication.Name = dto.MedicationName ?? medication.Name;
                    medication.Frequency = dto.Frequency ?? medication.Frequency;
                }

                // Update medical details
                foreach (var medicalDetail in patient.MedicalDetails)
                {
                    medicalDetail.Value = dto.Value ?? medicalDetail.Value;
                    // Update other medical detail properties as needed
                }



                // Save changes to the repository
                _repository.Update(patient);

                // Create and return a DTO representing the updated patient
                var updateDTO = new UpdatePatientFromDoctorDto
                {
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    BloodGroup = patient.BloodGroup,
                    PrimaryPhysicanName = patient.PrimaryPhysicanName,
                    PrimaryPhysicanEmail = patient.PrimaryPhysicanEmail,
                    PrimaryPhysicanPhoneNo = patient.PrimaryPhysicanPhoneNo,
                    EmergencyContactName = patient.EmergencyContactName,
                    EmergencyContactPhoneNo = patient.EmergencyContactPhoneNo,
                    EmergencyContactRelationship = patient.EmergencyContactRelationship,
                    Height = patient.Height,
                    Weight = patient.Weight,
                    FirstName = patient.User.FirstName,
                    MiddleName = patient.User.MiddleName,
                    LastName = patient.User.LastName,
                    Email = patient.User.Email,
                    PhoneNumber = patient.User.PhoneNumber,
                    ImageUrl = patient.User.ImageUrl,
                    // Add other properties as needed
                };

                if (patient.Medications.Any())
                {
                    updateDTO.Dosage = patient.Medications.First().Dosage;
                    updateDTO.Frequency = patient.Medications.First().Frequency;
                    updateDTO.MedicationName = patient.Medications.First().Name;
                }

                if (patient.MedicalDetails.Any())
                {
                    updateDTO.Value = patient.MedicalDetails.First().Value;
                }

                return Result.Success(updateDTO);
            }
            catch (Exception ex)
            {
                return Result.Failure<UpdatePatientFromDoctorDto>(new[] { new Error("Exception", ex.Message) });
            }
        }
    }
}