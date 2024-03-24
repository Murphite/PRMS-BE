using PRMS.Core.Dtos;
using PRMS.Domain.Enums;

namespace PRMS.Core.Abstractions;

public interface IPrescriptionService
{
    public Task<Result> CreatePrescription(string patientUserId, string physicianUserId,
        CreatePrescriptionDto prescriptionDto);
    public Task<Result> UpdatePrescription(string medicationId, MedicationStatus medicationStatus);
}