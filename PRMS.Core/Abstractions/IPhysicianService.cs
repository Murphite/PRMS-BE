using PRMS.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRMS.Core.Abstractions;

public interface IPhysicianService
{
    public Task<Result<PaginatorDto<IEnumerable<PhysicianReviewDto>>>> GetReviews(string physicianId,
        PaginationFilter paginationFilter);

    public Task<Result<PhysicianDetailsDto>> GetDetails(string physicianId);
    public Task<Result<PaginatorDto<IEnumerable<GetPhysiciansDTO>>>> GetAll(PaginationFilter paginationFilter);   
}
