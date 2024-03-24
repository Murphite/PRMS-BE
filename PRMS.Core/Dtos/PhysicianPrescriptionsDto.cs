using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
	public class PhysicianPrescriptionsDto
	{
		public string date { get; set; }
		public string patientName { get; set; }
		public string medicationName { get; set; }
		public string dosage { get; set; }
		public string? instructions { get; set; }
		//public string status { get; set; }
	}
}
