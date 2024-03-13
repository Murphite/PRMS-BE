using PRMS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
    public record PhysicianPatientsAppointmentsDto
    (
        string PatientName,
        string PatientEmail,
        string? PatientImageUrl,
        DateTimeOffset Date,
        string PatientBloodGroup,
        float PatientHeight,
        float PatientWeight,
        string? PrimaryPhysicanName,
        List<string> Diagnosis,
        List<PatientMedication> MedicationUsage
    );

    public record PatientMedication(
        string Dosage,
        string Name,
        string Frequency
        );
}