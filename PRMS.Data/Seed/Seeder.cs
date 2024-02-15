using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PRMS.Data.Contexts;
using PRMS.Domain.Constants;
using PRMS.Domain.Entities;

namespace PRMS.Data.Seed;

public static class Seeder
{
    public static async Task Run(IApplicationBuilder app)
    {
        var context = app.ApplicationServices.CreateScope().ServiceProvider
            .GetRequiredService<AppDbContext>();
        if ((await context.Database.GetPendingMigrationsAsync()).Any())
            await context.Database.MigrateAsync();

        if (!context.Roles.Any())
        {
            var roles = new List<IdentityRole>
            {
                new() { Name = RolesConstant.User, NormalizedName = RolesConstant.User },
                new() { Name = RolesConstant.Admin, NormalizedName = RolesConstant.Admin }
            };
            await context.Roles.AddRangeAsync(roles);
            await context.SaveChangesAsync();

            var userManager = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<UserManager<User>>();
            var user = User.Create("Admin", "Admin", "admin@admin.com");

            await userManager.CreateAsync(user, "Admin@123");
            await userManager.AddToRoleAsync(user, RolesConstant.Admin);
        }
    }
}