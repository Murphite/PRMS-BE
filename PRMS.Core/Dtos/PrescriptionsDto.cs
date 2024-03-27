using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
	public class PrescriptionsDto
	{
		public string MedicationId { get; set; }
		public string Date { get; set; }
		public string PatientName { get; set; }
		public string MedicationName { get; set; }
		public string Dosage { get; set; }
		public string? Instructions { get; set; }
		public string MedicationStatus { get; set; }
	}
}
