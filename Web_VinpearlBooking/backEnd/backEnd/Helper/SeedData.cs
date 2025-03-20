using Database.Data;
using Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebHotel.Commom;

namespace WebHotel.Helper
{
    public class SeedData
    {
        public static async Task SeedDataAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            string[] roles = { UserRoles.Admin, UserRoles.User, UserRoles.Manager, UserRoles.Employee };

            var dbContext = serviceProvider.GetRequiredService<MyDBContext>();
            if (await dbContext.Users.AnyAsync() || await dbContext.Roles.AnyAsync())
            {
                return;
            }
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole(role));
                }
            }
            var users = new[]
            {
                new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    Name = "Admin User",
                    Address = "123 Admin Street",
                    CMND = "123456789",
                    Image = "admin.png",
                    CreatedAt = DateTime.UtcNow
                },
                new ApplicationUser
                {
                    UserName = "user@example.com",
                    Email = "user@example.com",
                    Name = "Regular User",
                    Address = "456 User Avenue",
                    CMND = "987654321",
                    Image = "user.png",
                    CreatedAt = DateTime.UtcNow
                },
                new ApplicationUser
                {
                    UserName = "manager@example.com",
                    Email = "manager@example.com",
                    Name = "Manager User",
                    Address = "789 Manager Blvd",
                    CMND = "111222333",
                    Image = "manager.png",
                    CreatedAt = DateTime.UtcNow
                },
                new ApplicationUser
                {
                    UserName = "employee@example.com",
                    Email = "employee@example.com",
                    Name = "Employee User",
                    Address = "101 Employee Road",
                    CMND = "444555666",
                    Image = "employee.png",
                    CreatedAt = DateTime.UtcNow
                }
            };

            string defaultPassword = "Password@123";
            for (int i = 0; i < users.Length; i++)
            {
                var user = users[i];
                if (await userManager.FindByEmailAsync(user.Email) == null)
                {
                    var result = await userManager.CreateAsync(user, defaultPassword);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, roles[i]);
                    }
                }
            }
        }
    }
}
