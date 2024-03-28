using PRMS.Domain.Enums;

namespace PRMS.Core.Dtos;

    public class MonthlyAppointmentsDto
    {
        public int Date { get; set; }
        public int AppointmentCount { get; set; }
    }
