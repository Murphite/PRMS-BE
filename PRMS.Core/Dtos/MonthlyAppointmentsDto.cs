using PRMS.Domain.Enums;

namespace PRMS.Core.Dtos
{
    public class MonthlyAppointmentsDto
    {
        public string PatientName { get; set; }
        public AppointmentStatus Status { get; set; }
        public int Month { get; set; }
        public DateTimeOffset Date { get; set; }
        public string ImageUrl { get; set; }
    }
}
