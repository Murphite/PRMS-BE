using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface IPrescriptionService
{
    public Task<Result> CreatePrescription(string patientUserId, string physicianUserId,
        CreatePrescriptionDto prescriptionDto);
    public Task<Result<PaginatorDto<IEnumerable<PrescribedMedicationDto>>>> GetPatiencePrescribedMedicationHistory(string patientUserId, PaginationFilter paginationFilter);
}