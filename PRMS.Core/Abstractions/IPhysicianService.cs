using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

namespace PRMS.Core.Abstractions;

public interface IPhysicianService
{
    public Task<Result<IEnumerable<PhysicianReviewDto>>> GetReviews(string physicianId, PaginationFilter paginationFilter);
}