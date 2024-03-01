using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Utilities;
using PRMS.Data.Contexts;
using PRMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    ImageUrl = ms.User.ImageUrl,
                    Title = ms.Title,
                    Speciality = ms.Speciality,
                    Street = ms.User.Address.Street,
                    City = ms.User.Address.City,
                    State = ms.User.Address.State,
                    MedicalCenterName = ms.MedicalCenter.Name,
                    ReviewCount = ms.Reviews.Count(), // No need for null check here
                    Rating = (int)Math.Round(ms.Reviews.Average(r => r.Rating)) // No need for null check here
                });

            var paginatedMedicalSpecialists = await medicalSpecialistsQuery.Paginate(paginationFilter);

            return Result.Success(paginatedMedicalSpecialists);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginatorDto<IEnumerable<GetMedicalSpecialistDTO>>>(new[] { new Error(ex.Message, "EXCEPTION") });
        }
    }

}
