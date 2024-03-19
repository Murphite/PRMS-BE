using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

namespace PRMS.Core.Services;

public class PrescriptionService : IPrescriptionService
{
    private readonly IRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public PrescriptionService(IRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> CreatePrescription(string patientUserId, string physicianUserId, CreatePrescriptionDto prescriptionDto)
    {
        var patientId = await _repository.GetAll<Patient>()
            .Where(p => p.UserId == patientUserId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();
        
        if(patientId is null)
        {
            return new Error[] { new("Patient.Error", "Patient not found") };
        }
        
        var physicianId = await _repository.GetAll<Physician>()
            .Where(p => p.UserId == physicianUserId)
            .Select(p => p.Id)
            .FirstOrDefaultAsync();
        
        if(physicianId is null)
            return new Error[] {new("Physician.Error", "Physician not found")};
        
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
}