using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface IPrescriptionService
{
    public Task<Result> CreatePrescription(string patientUserId, string physicianUserId,
        CreatePrescriptionDto prescriptionDto);
}