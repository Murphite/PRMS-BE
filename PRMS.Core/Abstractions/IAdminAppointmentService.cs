using PRMS.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Abstractions
{
    public interface IAdminAppointmentService
    {
        public Task<Result> GetPatientAppointments(string userId, string status, PaginationFilter paginationFilter);
    }
}
