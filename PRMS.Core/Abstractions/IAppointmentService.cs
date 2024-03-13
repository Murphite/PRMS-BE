using PRMS.Core.Dtos;
using PRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Abstractions
{
    public interface IAppointmentService
    {
        public Task<Result> GetAppointmentsForPhysician(string physicianId, DateTimeOffset startDate, DateTimeOffset endDate);
    }
}
