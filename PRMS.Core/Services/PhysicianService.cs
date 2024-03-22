using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Utilities;
using PRMS.Domain.Entities;

namespace PRMS.Core.Services;

public class PhysicianService : IPhysicianService
{
    private readonly IRepository _repository;

    public PhysicianService(IRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<PaginatorDto<IEnumerable<PhysicianReviewDto>>>> GetReviews(string physicianId,
        PaginationFilter paginationFilter)
    {
        var reviews = await _repository.GetAll<PhysicianReview>()
            .Where(p => p.PhysicianId == physicianId && p.Content != null)
            .Include(p => p.Patient)
            .ThenInclude(p => p.User)
            .OrderByDescending(p => p.CreatedAt)
            .Select(r => new PhysicianReviewDto
            {
                Content = r.Content,
                Rating = r.Rating,
                Name = r.Patient.User.FirstName + " " + r.Patient.User.LastName,
                Image = r.Patient.User.ImageUrl
            }).Paginate(paginationFilter);

        return reviews;
    }
    
    public async Task<Result<PhysicianDetailsDto>> GetDetails(string physicianId)
    {
        var result = await _repository.GetAll<Physician>()
            .Where(p => p.Id == physicianId).ToListAsync();
        
        var physician = await _repository.GetAll<Physician>()
            .Where(p => p.Id == physicianId)
            .Include(p => p.User)
            .Include(p => p.MedicalCenter)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync();

        if (physician is null)
            return new Error[] { new("Physician.NotFound", "Physician not found") };

        return new PhysicianDetailsDto();
    } 
}