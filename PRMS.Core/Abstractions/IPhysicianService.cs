using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

namespace PRMS.Core.Abstractions;

public interface IPhysicianService
{
    public Task<Result<IEnumerable<PhysicianReview>>> GetReviews(string physicianId);
}