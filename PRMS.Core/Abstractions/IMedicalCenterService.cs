using PRMS.Core.Dtos;
using PRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Abstractions
{
    public interface IMedicalCenterService
    {
        Task<Result<PaginatorDto<IEnumerable<GetMedicalCenterDto>>>> GetAll(string userId, double? userLatitude, double? userLongitude, PaginationFilter paginationFilter);
    }
}
