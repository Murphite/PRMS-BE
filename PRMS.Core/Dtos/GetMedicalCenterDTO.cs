using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
    public class GetMedicalCenterDTO
    {
        public string Name { get; set; }
        public string? ImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public double Distance { get; set; }
        public List<string> Categories { get; set; }
        public int ReviewCount { get; set; }
        public int? Rating { get; set; }
    }
}