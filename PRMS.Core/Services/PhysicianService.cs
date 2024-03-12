using Bogus.DataSets;
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

    public async Task<IEnumerable<PhysicianDetailsDto>> GetDetails(string physicianId)
    {
        var physician = await _repository.GetAll<Physician>()
            .Where(p => p.Id == physicianId)
            .Include(p => p.User)
            .Include(p => p.MedicalCenter)
            .Include(p => p.Reviews)
            .Select(p => new PhysicianDetailsDto
            {
                Name = $"{p.Title} {p.User.FirstName} {p.User.LastName}",
                Speciality = p.Speciality,
                About = p.About,
                WorkingTime = p.WorkingTime,
                YearsOfExperience = p.YearsOfExperience,
                AverageRating = p.Reviews.Average(r => r.Rating),
                ReviewCount = p.Reviews.Count(),
                PatientCount = p.Patients.Count(),
                ImageUrl = p.User.ImageUrl,
                MedicalCenterName = p.MedicalCenter.Name,
                MedicalCenterAddress = $"{p.MedicalCenter.Address.Street}, {p.MedicalCenter.Address.City}, {p.MedicalCenter.Address.State}"
            })
            .ToListAsync();
        return physician;
    } 
}


