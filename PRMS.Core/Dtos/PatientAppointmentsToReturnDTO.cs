using PRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
	//public class PatientAppointmentsToReturnDTO
	//{
 //       public List<AppointmentToReturnDto> UpcomingAppointments { get; set; }
	//	public List<AppointmentToReturnDto> CompletedAppointments { get; set; }
	//	public List<AppointmentToReturnDto> CancelledAppointments { get; set; }
 //       public string patientImage {  get; set; }

 //   }
    public record AppointmentToReturnDto(
        string Name,
		string PhysicianSpeciality,
		string PhysicianImageUrl,
		string PhysicianMedicalCenter,
	    string PhysicianAddress,
		DateTimeOffset AppointmentDate

		);
	
	
}
