namespace PRMS.Core.Dtos;

public record PhysicianPatientsAppointmentsDto(
    string PatientName,
    string PatientEmail,
    string? PatientImageUrl,
    DateTimeOffset Date,
    string PatientBloodGroup,
    float PatientHeight,
    float PatientWeight,
    string? PrimaryPhysicanName,
    IEnumerable<string> Diagnosis,
    IEnumerable<PatientMedication> MedicationUsage
);

public record PatientMedication(
    string Dosage,
    string Name,
    string Frequency
);