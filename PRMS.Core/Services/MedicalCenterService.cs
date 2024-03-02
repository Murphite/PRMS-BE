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
                    City = mc.Address.City,
                    State = mc.Address.State,
                    ReviewCount = mc.Reviews.Count(),
                    Rating = (int)Math.Round(mc.Reviews.Average(r => r.Rating)),
                    Distance = userLatitude.HasValue && userLongitude.HasValue
                        ? GeoCalculator.CalculateDistance(userLatitude.Value, userLongitude.Value, mc.Address.Latitude ?? 0, mc.Address.Longitude ?? 0)
                        : 0, // Default distance if user's location is not available
                    Categories = mc.CategoryPivot.Select(cp => cp.MedicalCenterCategory.Name).ToList()
                });

            var paginatedMedicalCenters = await medicalCentersQuery.Paginate(paginationFilter);

            return Result.Success(paginatedMedicalCenters);
        }
        catch (Exception ex)
        {
            return Result.Failure<PaginatorDto<IEnumerable<GetMedicalCenterDTO>>>(new[] { new Error(ex.Message, "EXCEPTION") });
        }
    }

    public static class GeoCalculator
    {
        private const double EarthRadiusKm = 6371;

        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            var distance = EarthRadiusKm * c;
            return distance;
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }
}

