namespace PRMS.Core.Dtos;

public class PatientDto
{
    public string PatientId { get; set; }
    public string PatientName { get; set; }
    public string? ImageUrl { get; set; }
    public DateTimeOffset DateCreated { get; set; }
    public int NoOfAppointments { get; set; }
}

public class ListPatientDto
{
    public ICollection<PatientDto> Patients { get; set; }
}