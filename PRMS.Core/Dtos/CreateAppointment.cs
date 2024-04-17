namespace PRMS.Core.Dtos;

public record CreateAppointmentDto(string PhysicianUserId, DateTimeOffset Date, string? Reason);