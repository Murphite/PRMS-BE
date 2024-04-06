using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
    public class GetPhysiciansDTO
    {
        //PHYSICICAN USER DETAILS
        public string PhysicianUserId { get; set; }
        public string PhysicianId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? ImageUrl { get; set; }

        //PHYSICAN DETAILS
        public string Title { get; set; }
        public string Speciality { get; set; }

        //PHYSICIAN REVIEWS
        public int ReviewCount { get; set; }
        public int? Rating { get; set; }


        //PHYSICIAN ADDRESS
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }

        //PHYSICIAN MEDICAL CENTER
        public string? MedicalCenterName { get; set; }


    }
}
