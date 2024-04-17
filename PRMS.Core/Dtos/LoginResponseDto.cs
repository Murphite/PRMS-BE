namespace PRMS.Core.Dtos;

public record LoginResponseDto(string Token, string Role, string? PatientId);