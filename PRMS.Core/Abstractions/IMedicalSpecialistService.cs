using PRMS.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Abstractions
{
    public interface IMedicalSpecialistService
    {
        Task<Result<PaginatorDto<IEnumerable<GetMedicalSpecialistDTO>>>> GetAll(string userId, PaginationFilter paginationFilter);
    }
}
