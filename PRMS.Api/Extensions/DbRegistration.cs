using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PRMS.Data.Contexts;
using PRMS.Domain.Entities;

namespace PRMS.Api.Extensions;

public static class DbRegistration
{
    public static void AddDbServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(typeof(AppDbContext).Assembly.GetName().Name);
                    // optionsBuilder.UseNetTopologySuite();
                }));

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
    }
}