using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface IPhysicianService
{ 
    public Task<PaginatorDto<IEnumerable<PhysicianReviewDto>>> GetReviews(string physicianId, PaginationFilter paginationFilter);
}