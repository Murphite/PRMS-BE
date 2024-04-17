using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Utilities;
using PRMS.Domain.Entities;

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
        var physicianDetails = await _repository.GetAll<Physician>()
            .Where(p => p.Id == physicianId)
            .Include(p => p.User)
            .Include(p => p.MedicalCenter)
            .Include(p => p.Reviews)
            .FirstOrDefaultAsync();
        
        if (physicianDetails == null)
            return new Error[] { new("Physician.Error", "No record of Physician Detail") };

        var physicianDetailsToReturn = new PhysicianDetailsDto
        {
            PhysicianId = physicianDetails.Id,
            PhysicianUserId = physicianDetails.User.Id,
            Name = $"{physicianDetails.User.FirstName} {physicianDetails.User.LastName}",
            Title = physicianDetails.Title,
            ImageUrl = physicianDetails.User.ImageUrl,
            // PatientCount = physicianDetails.Patients == null ? 0 : physicianDetails.Patients.Count,
            YearsOfExperience = physicianDetails.YearsOfExperience,
            MedicalCenterName = physicianDetails.MedicalCenter.Name,
            MedicalCenterAddress =
                $"{physicianDetails.MedicalCenter?.Address?.Street} {physicianDetails.MedicalCenter?.Address?.City} {physicianDetails.MedicalCenter?.Address?.State} {physicianDetails.MedicalCenter?.Address?.Country}",
            About = physicianDetails.About,
            WorkingTime = physicianDetails.WorkingTime,
            Speciality = physicianDetails.Speciality,
            ReviewCount = physicianDetails.Reviews.Count,
            // AverageRating = (int)Math.Round(physicianDetails.Reviews.Average(r => r.Rating))
        };


        //if (physician is null)
        //          return new Error[] { new("Physician.NotFound", "Physician not found") };

        return physicianDetailsToReturn;
    }

    public async Task<Result<PaginatorDto<IEnumerable<GetPhysiciansDTO>>>> GetAll(PaginationFilter paginationFilter)
    {
        var physicians = await _repository.GetAll<Physician>()
            .Include(physician => physician.Reviews)
            .Include(physician => physician.MedicalCenter)
            .Include(physician => physician.User)
            .ThenInclude(user => user.Address)
            .Select(physician => new GetPhysiciansDTO
            {
                PhysicianUserId = physician.User.Id,
                PhysicianId = physician.Id,
                FirstName = physician.User.FirstName,
                LastName = physician.User.LastName,
                MiddleName = physician.User.MiddleName,
                ImageUrl = physician.User.ImageUrl,
                Title = physician.Title,
                Speciality = physician.Speciality,
                Street = physician.User.Address.Street,
                City = physician.User.Address.City,
                State = physician.User.Address.State,
                MedicalCenterName = physician.MedicalCenter.Name,
                ReviewCount = physician.Reviews.Count,
                Rating = (int)Math.Round(physician.Reviews.Average(r => r.Rating))
            })
            .Paginate(paginationFilter);

        return physicians;
    }

    public async Task<Result<PaginatorDto<IEnumerable<PrescriptionsDto>>>> FetchPrescriptions(
        string userId, PaginationFilter paginationFilter)
    {
        var physicianUser = await _userManager.FindByIdAsync(userId);
        if (physicianUser is null)
            return new Error[] { new("User.NotFound", "User not found") };
        var physicianId = await _repository.GetAll<Physician>()
            .Where(p => p.UserId == userId)
            .Select(p => p.Id)
            .FirstAsync();

        var physicianPrescriptions = await _repository.GetAll<Medication>()
            .Where(m => m.Prescription != null && m.Prescription.PhysicianId == physicianId)
            .Include(m => m.Prescription)
            .Include(m => m.Patient)
            .ThenInclude(m => m.User)
            .Select(m => new PrescriptionsDto
            {
                MedicationId = m.Id,
                Date = m.CreatedAt.ToString("MMMM dd,yyyy"),
                PatientName = $"{m.Patient.User.FirstName} {m.Patient.User.LastName}",
                MedicationName = m.Name,
                Dosage = m.Dosage,
                Instructions = m.Instruction,
                MedicationStatus = m.MedicationStatus.ToString(),
            })
            .Paginate(paginationFilter);

        return physicianPrescriptions;
    }
}