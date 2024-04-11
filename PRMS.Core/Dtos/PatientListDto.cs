namespace PRMS.Core.Dtos;

public class PatientDto
{
    public string UserId { get; set; }
    public string PatientId { get; set; }
    public string PatientName { get; set; }
    public string? ImageUrl { get; set; }
    public string Email { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string AppointmentDate { get; set; }
    public int NoOfAppointments { get; set; }
}