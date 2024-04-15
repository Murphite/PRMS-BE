using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Utilities;
using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Core.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IRepository _repository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public PrescriptionService(IRepository repository, UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> CreatePrescription(string patientUserId, string physicianUserId,
        CreatePrescriptionDto prescriptionDto)
    {
        var patientId = await _repository.GetAll<Patient>()
            .Where(p => p.UserId == patientUserId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();

        if (patientId is null)
        {
            return new Error[] { new("Patient.Error", "Patient not found") };
        }

        var physicianId = await _repository.GetAll<Physician>()
            .Where(p => p.UserId == physicianUserId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();

        if (physicianId is null)
            return new Error[] { new("Physician.Error", "Physician not found") };

        var prescription = new Prescription
        {
            PatientId = patientId,
            PhysicianId = physicianId,
            Diagnosis = prescriptionDto.Diagnosis,
            Note = prescriptionDto.Instruction,
        };

        var medication = new Medication
        {
            PrescriptionId = prescription.Id,
            PatientId = patientId,
            Name = prescriptionDto.Name,
            Dosage = prescriptionDto.Dosage,
            Frequency = prescriptionDto.Frequency,
            Duration = prescriptionDto.Duration,
            Instruction = prescriptionDto.Instruction
        };

        await _repository.Add(prescription);
        await _repository.Add(medication);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

    public async Task<Result<PaginatorDto<IEnumerable<PrescribedMedicationDto>>>>
        GetPatientPrescribedMedicationHistory(string patientUserId, PaginationFilter paginationFilter)
    {
        var patient = await _repository.GetAll<Patient>()
            .FirstOrDefaultAsync(x => x.UserId == patientUserId);
        if (patient == null)
        {
            return new Error[] { new("Patient.Error", "Patient not found") };
        }

        var patientPrescribedMedicationHistory = await _repository
            .GetAll<Medication>()
            .Where(x => x.PatientId == patient.Id)
            .OrderByDescending(x => x.CreatedAt)
            .Select(m => new PrescribedMedicationDto
            {
                MedicationName = m.Name,
                Dosage = m.Dosage,
                Date = m.CreatedAt.ToString("MMMM dd, yyyy"),
                Instruction = m.Instruction,
                Status = m.MedicationStatus.ToString(),
            }).Paginate(paginationFilter);
        
        return Result.Success(patientPrescribedMedicationHistory);
    }

    public async Task<Result<PrescribedMedicationDto>> GetPrescribedMedicationHistoryById(string medicationId)
    {
        if (string.IsNullOrEmpty(medicationId))
        {
            return new Error[] { new("Medication.Error", "Invalid input") };
        }

        var medication = await _repository.GetAll<Medication>()
            .FirstOrDefaultAsync(x => x.Id == medicationId);

        if (medication == null)
        {
            return new Error[] { new("Medication.Error", "Medication not found") };
        }

        var medicationHistoryDto = new PrescribedMedicationDto
        {
            Id = medication.Id,
            PhysicianName = medication.Prescription?.Physician.Title + " " + medication.Prescription?.Physician?.User.FirstName + " " + medication.Prescription?.Physician?.User.LastName,
            Frequency = medication.Frequency,
            MedicationName = medication.Name,
            Dosage = medication.Dosage,
            Duration = medication.Duration,
            Instruction = medication.Instruction,
        };

        return Result.Success(medicationHistoryDto);
    }

    public async Task<Result> UpdateMedicationStatus(string medicationId, MedicationStatus medicationStatus)
    {
        var medication = await _repository.GetAll<Medication>().FirstOrDefaultAsync(x => x.Id == medicationId);

        if (medication == null)
        {
            return new Error[] { new("Medication.Error", "No Medication") };
        }

        medication.MedicationStatus = medicationStatus;

        _repository.Update(medication);
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    // public async Task<Result<PaginatorDto<IEnumerable<PrescriptionsDto>>>> FetchPrescriptionHistory(
    //     string physicianUserId, PaginationFilter paginationFilter)
    // {
    //     var prescriptions = await _repository.GetAll<Prescription>()
    //         .Where(p => p.PhysicianId == physicianUserId)
    //         .Include(p => p.Medications)
    //         .Select(p => new PrescriptionsDto
    //         {
    //             MedicationId = p.Me,
    //             Date = p.CreatedAt.ToString("MMMM dd,yyyy"),
    //             PatientName = $"{p.Patient.User.FirstName} {p.Patient.User.LastName}",
    //             MedicationName = p.Name,
    //             Dosage = p.Dosage,
    //             Instructions = p.Instruction,
    //             MedicationStatus = p.MedicationStatus.ToString(),
    //         })
    //         .Paginate(paginationFilter);
    //
    //     return physicianPrescriptions;
    // }
}