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
    public class MedicalSpecialistService : IMedicalSpecialistService
    {
        private readonly IRepository _repository;
        private readonly UserManager<User> _userManager;

        public MedicalSpecialistService(IRepository repository, UserManager<User> userManager)
        {
            _repository = repository;
            _userManager = userManager;
        }

        public async Task<Result<PaginatorDto<IEnumerable<GetMedicalSpecialistDTO>>>> GetAll(string userId, PaginationFilter paginationFilter)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return Result.Failure<PaginatorDto<IEnumerable<GetMedicalSpecialistDTO>>>(new[] { new Error("User.Error", "User Not Found") });
                }

                var medicalSpecialistsQuery = _repository.GetAll<Physician>()
                    .Select(ms => new GetMedicalSpecialistDTO
                    {
                        FirstName = ms.User.FirstName,
                        LastName = ms.User.LastName,
                        MiddleName = ms.User.MiddleName,
                        PhoneNumber = ms.User.PhoneNumber,
                        ImageUrl = ms.User.ImageUrl,

                        Title = ms.Title,
                        Speciality = ms.Speciality,
                        About = ms.About,
                        WorkingTime = ms.WorkingTime,
                        YearsOfExperience = ms.YearsOfExperience,

                        Street = ms.User.Address.Street,
                        City = ms.User.Address.City,
                        State = ms.User.Address.State,
                        Country = ms.User.Address.Country,

                        MedicalCenterName = ms.MedicalCenter.Name,

                        ReviewCount = ms.Reviews != null ? ms.Reviews.Count() : 0,
                        Rating = ms.Reviews != null ? (int?)Math.Round(ms.Reviews.Average(r => r.Rating)) : null,
                    });;



                var medicalSpecialists = await medicalSpecialistsQuery.ToListAsync();
                if (!medicalSpecialists.Any())
                {
                    // Return a success result with an empty list
                    return Result.Success(new PaginatorDto<IEnumerable<GetMedicalSpecialistDTO>>
                    {
                        PageItems = Enumerable.Empty<GetMedicalSpecialistDTO>(),
                        PageSize = 0,
                        CurrentPage = paginationFilter.PageNumber,
                        NumberOfPages = paginationFilter.PageSize
                    });
                }

                var paginatedMedicalSpecialists = await medicalSpecialistsQuery.Paginate(paginationFilter);

                return Result.Success(paginatedMedicalSpecialists);
            }
            catch (Exception ex)
            {
                return Result.Failure<PaginatorDto<IEnumerable<GetMedicalSpecialistDTO>>>(new[] { new Error(ex.Message, "EXCEPTION") });
            }
        }


    }

}
