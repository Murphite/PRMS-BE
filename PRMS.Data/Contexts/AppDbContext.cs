using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRMS.Domain.Entities;

namespace PRMS.Data.Contexts;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<MedicalCenter> MedicalCenters { get; set; }
    public DbSet<MedicalCenterCategory> MedicalCenterCategories { get; set; }
    public DbSet<MedicalCenterReview> MedicalCenterReviews { get; set; }
    public DbSet<Patient> Patients { get; set; }
    public DbSet<Physician> Physicians { get; set; }
    public DbSet<PhysicianReview> PhysicianReviews { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<MedicalDetail> MedicalDetails { get; set; }
    public DbSet<Medication> Medications { get; set; }
    public DbSet<Prescription> Prescriptions { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MedicalCenter>()
            .HasMany(mc => mc.MedicalCenterCategories)
            .WithMany(mcc => mcc.MedicalCenters)
            .UsingEntity(j => j.ToTable("MedicalCenterCategoryPivot"));
    }
}