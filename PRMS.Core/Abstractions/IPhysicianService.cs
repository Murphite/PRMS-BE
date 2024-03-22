using PRMS.Core.Dtos;

namespace PRMS.Core.Abstractions;

public interface IPhysicianService
{
    public Task<Result<PaginatorDto<IEnumerable<PhysicianReviewDto>>>> GetReviews(string physicianId,
        PaginationFilter paginationFilter);

    public Task<Result<PhysicianDetailsDto>> GetDetails(string physicianId);

    public Task<Result<PaginatorDto<IEnumerable<PhysicianPrescriptionsDto>>>> FetchPhysicianPrescriptions(string physicianUserId, PaginationFilter paginationFilter);
}