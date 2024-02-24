using System.Collections;
using Bogus;
using Microsoft.AspNetCore.Identity;
using PRMS.Data.Contexts;
using PRMS.Domain.Constants;
using PRMS.Domain.Entities;
using PRMS.Domain.Enums;

namespace PRMS.Data.Seed;

public class DataGenerator
{
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;

    private readonly List<Patient> _patients = new();
    private readonly List<Physician> _physicians = new();
    private readonly List<MedicalCenterCategory> _categories = new();

    public DataGenerator(AppDbContext context, UserManager<User> userManager)
    {
        Randomizer.Seed = new Random(54093);
        _context = context;
        _userManager = userManager;
    }

    public async Task Run()
    {
        await GenerateCategories();
        await GeneratePatients(100);
        await GeneratePhysicians(10);
        await GeneratePrescriptions();

        await _context.SaveChangesAsync();
    }

    private async Task GenerateCategories()
    {
        var categories = new List<MedicalCenterCategory>
        {
            new() { Name = "dentistry", ImageUrl = "/assets/vectors/dentistry.svg" },
            new() { Name = "cardiologist", ImageUrl = "/assets/vectors/cardiologist.svg" },
            new() { Name = "pulmonologist", ImageUrl = "/assets/vectors/pulmonologist.svg" },
            new() { Name = "general", ImageUrl = "/assets/vectors/general.svg" },
            new() { Name = "neurology", ImageUrl = "/assets/vectors/neurology.svg" },
            new() { Name = "gastroenterologist", ImageUrl = "/assets/vectors/gastroenterologist.svg" },
            new() { Name = "laboratories", ImageUrl = "/assets/vectors/laboratories.svg" },
            new() { Name = "vaccination", ImageUrl = "/assets/vectors/vaccination.svg" },
        };

        _categories.AddRange(categories);
        await _context.MedicalCenterCategories.AddRangeAsync(categories);
    }

    private async Task<IEnumerable<Address>> GenerateAddresses(int count)
    {
        var addresses = new List<Address>();

        var addressFaker = new Faker<Address>()
            .RuleFor(a => a.Street, f => f.Address.StreetAddress())
            .RuleFor(a => a.City, f => f.Address.City())
            .RuleFor(a => a.State, f => f.Address.State())
            .RuleFor(c => c.Country, f => "Nigeria")
            .RuleFor(c => c.Latitude, f => f.Address.Latitude())
            .RuleFor(c => c.Longitude, f => f.Address.Longitude());

        for (var i = 0; i < count; i++)
        {
            var address = addressFaker.Generate();
            await _context.Addresses.AddAsync(address);
            addresses.Add(address);
        }

        await _context.SaveChangesAsync();
        return addresses;
    }

    private async Task<IEnumerable<User>> GenerateUsers(int count)
    {
        var userAddresses = await GenerateAddresses(count);
        var users = new List<User>();

        foreach (var address in userAddresses)
        {
            var user = new Faker<User>()
                .RuleFor(u => u.FirstName, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.UserName, (f, u) => u.Email)
                .RuleFor(u => u.PhoneNumber, f => f.Person.Phone)
                .RuleFor(u => u.AddressId, address.Id)
                .RuleFor(u => u.ImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(u => u.PublicId, Guid.NewGuid().ToString())
                .Generate();

            await _userManager.CreateAsync(user, "Password@123");
            await _userManager.AddToRoleAsync(user, RolesConstant.User);
            users.Add(user);
        }

        return users;
    }

    private async Task GeneratePatients(int count)
    {
        var users = await GenerateUsers(count);

        foreach (var user in users)
        {
            var patient = new Faker<Patient>()
                .RuleFor(p => p.UserId, user.Id)
                .RuleFor(p => p.DateOfBirth, f => f.Date.PastDateOnly(25))
                .RuleFor(p => p.Gender, f => f.PickRandom(new[] { Gender.Female, Gender.Male }))
                .RuleFor(p => p.BloodGroup, f => f.PickRandom<BloodGroup>())
                .RuleFor(p => p.Height, f => f.Random.Float(150, 200))
                .RuleFor(p => p.Weight, f => f.Random.Float(50, 200))
                .Generate();

            var detail = new Faker<MedicalDetail>()
                .RuleFor(d => d.PatientId, patient.Id)
                .RuleFor(d => d.MedicalDetailsType, MedicalDetailsType.Allergy)
                .RuleFor(d => d.Value, f => f.Lorem.Sentence(2))
                .Generate();
            
            var detail2 = new Faker<MedicalDetail>()
                .RuleFor(d => d.PatientId, patient.Id)
                .RuleFor(d => d.MedicalDetailsType, MedicalDetailsType.MedicalCondition)
                .RuleFor(d => d.Value, f => f.Lorem.Sentence(2))
                .Generate();

            _patients.Add(patient);
            await _context.Patients.AddAsync(patient);
            await _context.MedicalDetails.AddAsync(detail);
            await _context.MedicalDetails.AddAsync(detail2);
        }
    }

    private async Task<MedicalCenter> GenerateMedicalCenter()
    {
        var address = (await GenerateAddresses(1)).First();

        var medicalCenter = new Faker<MedicalCenter>()
            .RuleFor(m => m.Name, f => f.Company.CompanyName())
            .RuleFor(m => m.AddressId, address.Id)
            .RuleFor(m => m.ImageUrl, f => f.Image.PicsumUrl())
            .RuleFor(m => m.PublicId, Guid.NewGuid().ToString())
            .RuleFor(m => m.Type, f => f.PickRandom<MedicalCenterType>())
            .Generate();

        var pivot = new Faker<CategoryMedicalCenterPivot>()
            .RuleFor(p => p.MedicalCenterId, medicalCenter.Id)
            .RuleFor(p => p.MedicalCenterCategoryId, f => f.PickRandom(_categories).Id)
            .Generate(2);

        await _context.MedicalCenters.AddAsync(medicalCenter);
        await _context.CategoryMedicalCenters.AddRangeAsync(pivot);

        return medicalCenter;
    }

    private async Task GeneratePhysicians(int count)
    {
        var users = await GenerateUsers(count);

        foreach (var user in users)
        {
            var medicalCenter = await GenerateMedicalCenter();

            var physicians = new Faker<Physician>()
                .RuleFor(p => p.UserId, user.Id)
                .RuleFor(p => p.MedicalCenterId, medicalCenter.Id)
                .RuleFor(p => p.Title, f => f.PickRandom(new[] { "Dr", "Nurse", "Pharm" }))
                .RuleFor(p => p.Speciality,
                    f => f.PickRandom(new[]
                    {
                        "Cardiologist", "Dentist", "Neurologist", "Surgeon", "Gynecologist", "Pediatrics",
                        "Orthoopedic Surgeon", "Psychiatrist"
                    }))
                .RuleFor(p => p.About, f => f.Lorem.Paragraphs(3))
                .RuleFor(p => p.WorkingTime, "Monday-Friday, 8am-6pm")
                .RuleFor(p => p.YearsOfExperience, f => f.Random.Int(2, 30))
                .Generate(2);

            _physicians.AddRange(physicians);
            await _context.Physicians.AddRangeAsync(physicians);
        }
    }

    private async Task GenerateMedications(string patientId, string prescriptionId, int count)
    {
        var medications = new Faker<Medication>()
            .RuleFor(m => m.PatientId, patientId)
            .RuleFor(m => m.PrescriptionId, prescriptionId)
            .RuleFor(m => m.Name, f => f.Commerce.ProductName())
            .RuleFor(m => m.Frequency, f => f.Lorem.Sentence(3))
            .RuleFor(m => m.Duration, f => $"{f.Random.Int(1, 30)} days")
            .RuleFor(m => m.Dosage, f => f.Lorem.Sentence())
            .RuleFor(m => m.Frequency, f => f.Lorem.Sentence())
            .RuleFor(m => m.Instruction, f => f.Lorem.Paragraphs(1))
            .Generate(count);

        await _context.Medications.AddRangeAsync(medications);
    }

    private async Task GeneratePrescriptions()
    {
        foreach (var patient in _patients)
        {
            var prescription = new Faker<Prescription>()
                .RuleFor(p => p.PatientId, patient.Id)
                .RuleFor(p => p.PhysicianId, f => f.PickRandom(_physicians).Id)
                .RuleFor(p => p.Note, f => f.Lorem.Paragraphs(1))
                .RuleFor(p => p.Diagnosis, f => f.Lorem.Sentence(2))
                .Generate();

            _context.Prescriptions.Add(prescription);

            await GenerateMedications(patient.Id, prescription.Id, 2);
        }
    }
}