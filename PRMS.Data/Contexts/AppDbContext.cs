using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PRMS.Domain.Entities;

namespace PRMS.Data.Contexts;

public class AppDbContext : IdentityDbContext<User>
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<MedicalCenter> Clinics { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}