using Microsoft.EntityFrameworkCore;

namespace IAM_API.Data.DbSeeder;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IAMContext context)
    {
        // Apply migrations
        await context.Database.MigrateAsync();

        // Run individual seeders
        await RoleSeeder.SeedAsync(context);
    }
}