using IAM_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace IAM_API.Data.DbSeeder;

public static class RoleSeeder
{
    public static async Task SeedAsync(IAMContext context)
    {
        var defaultRoles = new[]
        {
            new Role
            {
                Name = "Admin",
                Description = "Admin role with full access"
            },
            new Role
            {
                Name = "Employee",
                Description = "Employee role with limited access"
            }
        };

        foreach (var role in defaultRoles)
        {
            bool roleExists = await context.Roles
                .AnyAsync(r => r.Name == role.Name);

            if (!roleExists)
            {
                context.Roles.Add(role);
            }
        }

        await context.SaveChangesAsync();
    }
}
