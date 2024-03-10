using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Domain.Entities;

namespace PRMS.Core.Services;

public class PhysicianService : IPhysicianService
{
    private readonly IRepository _repository;

    public PhysicianService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<PhysicianReview>>> GetReviewsAsync(string physicianId)
    {
        var reviews = await _repository.GetAll<PhysicianReview>()
            .Where(review => review.PhysicianId == physicianId)
            .ToListAsync();

        return reviews;
    }
}

