using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
    public class PhysicianDetailsDto
    {
		public string PhysicianUserId { get; set; }
		public string PhysicianId { get; set; }
		public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        public int PatientCount { get; set; }
        public int YearsOfExperience { get; set; }
        public string MedicalCenterName { get; set; }
        public string MedicalCenterAddress { get; set; }
        public string About { get; set; }
        public string WorkingTime { get; set; }
        public string Speciality{ get; set;}
    }
}
