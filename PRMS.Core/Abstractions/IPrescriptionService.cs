using PRMS.Core.Dtos;
using PRMS.Domain.Enums;

namespace PRMS.Core.Abstractions;

public interface IPrescriptionService
{
    public Task<Result> CreatePrescription(string patientUserId, string physicianUserId,
        CreatePrescriptionDto prescriptionDto);
    public Task<Result<PaginatorDto<IEnumerable<PrescribedMedicationDto>>>> GetPatientPrescribedMedicationHistory(string patientUserId, PaginationFilter paginationFilter);
    public Task<Result> UpdateMedicationStatus(string medicationId, MedicationStatus medicationStatus);
}