namespace PRMS.Core.Dtos
{
    public class PhysicianRangedAppointmentDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }
        public DateTimeOffset Date { get; set; }
    }
}
