using PRMS.Domain.Entities;

namespace PRMS.Core.Dtos;

public record PhysicianPatientsAppointmentsDto(
    string Id,
    string PatientName,
    string PatientEmail,
    string? PatientImageUrl,
    string Date,
    string PatientBloodGroup,
    float PatientHeight,
    float PatientWeight,
    string? PrimaryPhysicanName,
    ICollection<MedicalDetail> PatientMedicalDetails,
    IEnumerable<string> Diagnosis,
    IEnumerable<PatientMedication> MedicationUsage,
    string StartTime,
    string EndTime);

public record PatientMedication(
    string Dosage,
    string Name,
    string Frequency
);