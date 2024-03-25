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

    public async Task<Result<PaginatorDto<IEnumerable<PhysicianPrescriptionsDto>>>> FetchPhysicianPrescriptions(string physicianUserId, PaginationFilter paginationFilter)
    {
		var physicianId = await _repository.GetAll<Physician>()
			.Where(p => p.UserId == physicianUserId)
            .Select(p=>p.Id)
			.FirstOrDefaultAsync();

        var physicianPrescriptions =  await _repository.GetAll<Medication>()
             .Include(m => m.Prescription)
             .Where(m => m.Prescription.PhysicianId == physicianId)
             .Include(m => m.Patient)
             .ThenInclude(m => m.User)
             .Select(m => new PhysicianPrescriptionsDto
             {
                 MedicationId = m.Id,
                 PrescriptionId = m.PrescriptionId,
                 Date = m.CreatedAt.ToString("MMMM dd,yyyy"),
                 PatientName = $"{m.Patient.User.FirstName} {m.Patient.User.LastName}",
                 MedicationName = m.Name,
                 Dosage = m.Dosage,
                 Instructions = m.Instruction,
                 MedicationStatus=m.MedicationStatus.ToString(),
             }).Paginate(paginationFilter);

        return physicianPrescriptions;
	}
}