namespace PRMS.Core.Dtos
{
	public record PatientAppointmentsToReturnDTO
		(
		string Name,
		string PhysicianSpeciality,
		string? PhysicianImageUrl,
		string PhysicianMedicalCenter,
		string PhysicianAddress,
		DateTimeOffset AppointmentDate
		);		
}
