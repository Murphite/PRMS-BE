using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Utilities;
using PRMS.Data.Contexts;
using PRMS.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRMS.Core.Services
{
    public class MedicalCenterService : IMedicalCenterService
    {
        private readonly IRepository _repository;
        private readonly UserManager<User> _userManager;

        public MedicalCenterService(IRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<Result<PaginatorDto<IEnumerable<GetMedicalCenterDTO>>>> GetAll(string userId, PaginationFilter paginationFilter)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Result.Failure<PaginatorDto<IEnumerable<GetMedicalCenterDTO>>>(new[] { new Error("User.Error", "User Not Found") });
                }

                var medicalCentersQuery = _repository.GetAll<MedicalCenter>()
                    .Select(mc => new GetMedicalCenterDTO
                    {
                        Name = mc.Name,
                        ImageUrl = mc.ImageUrl,
                        Street = mc.Address.Street,
                        City = mc.Address.City,
                        State = mc.Address.State,
                        Country = mc.Address.Country,
                        Latitude = mc.Address.Latitude,
                        Longitude = mc.Address.Longitude,
                        ReviewCount = mc.Reviews != null ? mc.Reviews.Count() : 0,
                        Rating = mc.Reviews != null ? (int?)Math.Round(mc.Reviews.Average(r => r.Rating)) : null,
                        Categories = mc.CategoryPivot.Select(cp => cp.MedicalCenterCategory.Name).ToList()

                    });



                var medicalCenters = await medicalCentersQuery.ToListAsync();
                if (!medicalCenters.Any())
                {
                    // Return a success result with an empty list
                    return Result.Success(new PaginatorDto<IEnumerable<GetMedicalCenterDTO>>
                    {
                        PageItems = Enumerable.Empty<GetMedicalCenterDTO>(),
                        PageSize = 0,
                        CurrentPage = paginationFilter.PageNumber,
                        NumberOfPages = paginationFilter.PageSize
                    });
                }

                var paginatedMedicalCenters = await medicalCentersQuery.Paginate(paginationFilter);

                return Result.Success(paginatedMedicalCenters);
            }
            catch (Exception ex)
            {
                return Result.Failure<PaginatorDto<IEnumerable<GetMedicalCenterDTO>>>(new[] { new Error(ex.Message, "EXCEPTION") });
            }
        }


    }

}
