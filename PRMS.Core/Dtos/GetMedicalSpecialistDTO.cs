using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
    public class GetMedicalSpecialistDTO
    {
        //PHYSICICAN USER DETAILS
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl { get; set; }

        //PHYSICAN DETAILS
        public string? Title { get; set; }
        public string? Speciality { get; set; }
        public string? About { get; set; }
        public string? WorkingTime { get; set; }
        public int? YearsOfExperience { get; set; }

        //PHYSICIAN REVIEWS
        public int? ReviewCount { get; set; }
        public int? Rating { get; set; }


        //PHYSICIAN ADDRESS
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        //PHYSICIAN MEDICAL CENTER
        public string? MedicalCenterName { get; set; }


    }
}
