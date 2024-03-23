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

public class MedicalCenterService : IMedicalCenterService
{
    private readonly IRepository _repository;
    private readonly UserManager<User> _userManager;

    public MedicalCenterService(IRepository repository, UserManager<User> userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }

    public async Task<Result<PaginatorDto<IEnumerable<GetMedicalCenterDTO>>>> GetAll(string userId, double? userLatitude, double? userLongitude, PaginationFilter paginationFilter)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return new Error[] { new("User.Error", "User Not Found") };
        }

        // Query medical centers
        var medicalCentersQuery = _repository.GetAll<MedicalCenter>();

        // If user's latitude and longitude are available, order by distance
        if (userLatitude.HasValue && userLongitude.HasValue)
        {
            medicalCentersQuery = medicalCentersQuery
                .Where(mc => mc.Address.Latitude != null && mc.Address.Longitude != null)
                .OrderBy(mc =>
                    GeoCalculator.CalculateDistance(userLatitude.Value, userLongitude.Value, mc.Address.Latitude.Value, mc.Address.Longitude.Value));
        }
        else
        {
            // If user's latitude and longitude are not available, do not order by distance
            medicalCentersQuery = medicalCentersQuery.OrderBy(mc => mc.Id); // Order by medical center ID
        }

        // Select medical center DTOs
        var nearbyMedicalCenters = await medicalCentersQuery
            .Select(mc => new GetMedicalCenterDTO
        {
                Name = mc.Name,
                ImageUrl = mc.ImageUrl,
                City = mc.Address.City,
                State = mc.Address.State,
                ReviewCount = mc.Reviews.Count(),
                Rating = (int)Math.Round(mc.Reviews.Average(r => r.Rating)),
                // Calculate distance using GeoCalculator
                Distance = GeoCalculator.CalculateDistance(userLatitude ?? 0, userLongitude ?? 0, mc.Address.Latitude ?? 0, mc.Address.Longitude ?? 0),
                Categories = mc.MedicalCenterCategories.Select(mcc => mcc.Name).ToList()
            })
            .Paginate(paginationFilter);

        return Result.Success(nearbyMedicalCenters);
    }
}

