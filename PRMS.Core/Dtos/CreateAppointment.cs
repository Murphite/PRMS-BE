namespace PRMS.Core.Dtos;

public record CreateAppointmentDto(string PhysicianId, DateOnly Date, TimeOnly Time, string? Reason);