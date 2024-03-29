namespace PRMS.Core.Dtos
{
    public class PrescribedMedicationDto
    {
        public string Id { get; set; }
        public string PhysicianName { get; set; }
        public string MedicationName { get; set; }
        public string Dosage { get; set; }
        public string? Instruction { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public string Frequency { get; set; }
        public string Duration { get; set; }

    }
}
