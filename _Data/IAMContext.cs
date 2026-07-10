using Microsoft.EntityFrameworkCore;
using IAM_API._Entities;

namespace IAM_API._Data;

public class IAMContext : DbContext
{
    public IAMContext(DbContextOptions<IAMContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
}