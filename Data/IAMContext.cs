using IAM_API.Entities;
using Microsoft.EntityFrameworkCore;


namespace IAM_API.Data;

public class IAMContext : DbContext
{
    public IAMContext(DbContextOptions<IAMContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<UserRole>()
        .HasKey(ur => new { ur.UserId, ur.RoleId });

       modelBuilder.Entity<User>()
        .HasMany(u => u.UserRoles)
        .WithOne(ur => ur.User)
        .HasForeignKey(ur => ur.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}