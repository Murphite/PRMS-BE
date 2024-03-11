﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRMS.Core.Abstractions;
using PRMS.Core.Dtos;
using PRMS.Core.Utilities;
using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Core.Services;

public class PatientService : IPatientService
{
    private readonly IRepository _repository;
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public PatientService(IRepository repository, UserManager<User> userManager, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> UpdateFromPatientAsync(UpdatePatientFromPatientDto dto, string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return new Error[] { new("User.Error", "User Not Found") };
        }

        var patient = _repository.GetAll<Patient>()
            .FirstOrDefault(x => x.UserId == user.Id);

        if (patient == null)
        {
            return new Error[] { new("Patients.Error", "Patient Not Found") };
        }

        patient.DateOfBirth = dto.DateOfBirth ?? patient.DateOfBirth;
        patient.Gender = dto.Gender ?? patient.Gender;
        patient.BloodGroup = dto.BloodGroup ?? patient.BloodGroup;
        patient.PrimaryPhysicanName = dto.PrimaryPhysicanName ?? patient.PrimaryPhysicanName;
        patient.PrimaryPhysicanEmail = dto.PrimaryPhysicanEmail ?? patient.PrimaryPhysicanEmail;
        patient.PrimaryPhysicanPhoneNo = dto.PrimaryPhysicanPhoneNo ?? patient.PrimaryPhysicanPhoneNo;
        patient.Height = dto.Height ?? patient.Height;
        patient.Weight = dto.Weight ?? patient.Weight;
        patient.EmergencyContactName = dto.EmergencyContactName ?? patient.EmergencyContactName;
        patient.EmergencyContactPhoneNo = dto.EmergencyContactPhoneNo ?? patient.EmergencyContactPhoneNo;
        patient.EmergencyContactRelationship = dto.EmergencyContactRelationship ?? patient.EmergencyContactRelationship;

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.MiddleName = dto.MiddleName ?? user.MiddleName;
        user.LastName = dto.LastName ?? user.LastName;
        user.PhoneNumber = dto.PhoneNumber ?? user.PhoneNumber;

        _repository.Update(patient);
        await _userManager.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }

	

	public async Task<Result> GetPatientAppointments(string userId, string? status, PaginationFilter paginationFilter)
	{
		var user = await _userManager.FindByIdAsync(userId);
		if (user == null)
			return new Error[] { new("User.Error", "User Not Found") };

		var patient = _repository.GetAll<Patient>().FirstOrDefault(x => x.UserId == user.Id);
		if (patient == null)
			return new Error[] { new("User.Error", "Patient Not Found") };
		var patientAppointments= _repository.GetAll<Appointment>().Where(a=>a.PatientId==patient.Id);
		if (status == null)
		{
			var allPatientAppointments =patientAppointments
									 .OrderByDescending(c => c.CreatedAt)
									 .Include(p => p.Physician)
									 .ThenInclude(u => u.User)
									 .Select(p => new AppointmentToReturnDto(
										 $"{p.Physician.User.FirstName} {p.Physician.User.LastName}",
										 p.Physician.Speciality,
										 p.Physician.User.ImageUrl,
										 p.Physician.MedicalCenter.Name,
										 $"{p.Physician.MedicalCenter.Address.Street} {p.Physician.MedicalCenter.Address.City} {p.Physician.MedicalCenter.Address.State}",
										 p.Date
										 ))
									 .Paginate(paginationFilter);
			if (allPatientAppointments is null)
				return new Error[] { new("Appointment.Error", "No Record of Appointment(s)") };
			return Result.Success(allPatientAppointments);
	    }
		if ((status.ToLower() == AppointmentStatus.Pending.ToString().ToLower() || status.ToLower() == "upcoming"))
		{
			var upcomingPatientAppointments = patientAppointments
									 .Where(s=>s.Status.ToString().ToLower()==status.ToLower())
									 .OrderByDescending(c => c.CreatedAt)
									 .Include(p => p.Physician)
									 .ThenInclude(u => u.User)
									 .Select(p => new AppointmentToReturnDto(
										 $"{p.Physician.User.FirstName} {p.Physician.User.LastName}",
										 p.Physician.Speciality,
										 p.Physician.User.ImageUrl,
										 p.Physician.MedicalCenter.Name,
										$"{p.Physician.MedicalCenter.Address.Street} {p.Physician.MedicalCenter.Address.City} {p.Physician.MedicalCenter.Address.State}",
										 p.Date
										 ))
									 .Paginate(paginationFilter);
			if (upcomingPatientAppointments is null)
				return new Error[] { new("Appointment.Error", "No Upcoming Appointment(s)") };
			return Result.Success(upcomingPatientAppointments);
		}
		if (status.ToLower() == AppointmentStatus.Completed.ToString().ToLower())
		{
			var completedPatientAppointments = patientAppointments
									 .Where(s=> s.Status.ToString().ToLower() == status.ToLower())
									 .OrderByDescending(c=>c.CreatedAt)
									 .Include(p => p.Physician)
									 .ThenInclude(u => u.User)
									 .Select(p => new AppointmentToReturnDto(
										 $"{p.Physician.User.FirstName} {p.Physician.User.LastName}",
										 p.Physician.Speciality,
										 p.Physician.User.ImageUrl,
										 p.Physician.MedicalCenter.Name,
										 $"{p.Physician.MedicalCenter.Address.Street} {p.Physician.MedicalCenter.Address.City} {p.Physician.MedicalCenter.Address.State}",
										 p.Date
										 ))
									 .Paginate(paginationFilter);
			if (completedPatientAppointments is null)
				return new Error[] { new("Appointment.Error", "No Completed Appointment(s)") };
			return Result.Success(completedPatientAppointments);
		
		}
		if (status.ToLower() == AppointmentStatus.Cancelled.ToString().ToLower())
		{
			var cancelledPatientAppointments = _repository.GetAll<Appointment>()
									 .Where(s=>s.Status.ToString().ToLower() == status.ToLower())
									 .OrderByDescending(c => c.CreatedAt)
									 .Include(p => p.Physician)
									 .ThenInclude(u => u.User)
									 .Select(p => new AppointmentToReturnDto(
										 $"{p.Physician.User.FirstName} {p.Physician.User.LastName}",
										 p.Physician.Speciality,
										 p.Physician.User.ImageUrl,
										 p.Physician.MedicalCenter.Name,
										 $"{p.Physician.MedicalCenter.Address.Street} {p.Physician.MedicalCenter.Address.City} {p.Physician.MedicalCenter.Address.State}",
										 p.Date
										 ))
									 .Paginate(paginationFilter);
			if (cancelledPatientAppointments is null)
				return new Error[] { new("Appointment.Error", "No Completed Appointment(s)") };
			return Result.Success(cancelledPatientAppointments);

		}
		return Result.Success();
	}

}