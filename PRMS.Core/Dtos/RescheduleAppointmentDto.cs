using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Dtos
{
    public record RescheduleAppointmentDto
     (
         DateTimeOffset NewAppointmentDate
     );
}
