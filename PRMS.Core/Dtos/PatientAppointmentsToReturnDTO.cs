using PRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
	public record PatientAppointmentsToReturnDTO
		(
		string Name,
		string PhysicianSpeciality,
		string PhysicianImageUrl,
		string PhysicianMedicalCenter,
		string PhysicianAddress,
		DateTimeOffset AppointmentDate
		);		
}
