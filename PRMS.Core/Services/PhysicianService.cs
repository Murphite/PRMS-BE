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

namespace PRMS.Core.Services;

public class PhysicianService : IPhysicianService
{
    private readonly IRepository _repository;
    private readonly UserManager<User> _userManager;

    public PhysicianService(IRepository repository, UserManager<User> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    public async Task<Result<PaginatorDto<IEnumerable<GetPhysiciansDTO>>>> GetAll(PaginationFilter paginationFilter)
    {
        var physicansQuery = _repository.GetAll<Physician>()
            .Select(ms => new GetPhysiciansDTO
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
                ReviewCount = ms.Reviews.Count(),
                Rating = (int)Math.Round(ms.Reviews.Average(r => r.Rating))
            });

        var paginatedPhysicans = await physicansQuery.Paginate(paginationFilter);

        return Result.Success(paginatedPhysicans);
    }


}
