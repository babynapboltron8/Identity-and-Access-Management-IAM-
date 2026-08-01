using IAM_API.Entities;
using Microsoft.EntityFrameworkCore;


namespace IAM_API.Data;

public class IAMContext : DbContext
{
    public IAMContext(DbContextOptions<IAMContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
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

       modelBuilder.Entity<User>()
        .HasIndex(u => u.Username)
        .IsUnique();

       modelBuilder.Entity<User>()
        .HasIndex(u => u.Email)
        .IsUnique();

       modelBuilder.Entity<Role>()
        .HasIndex(r => r.Name)
        .IsUnique();

       modelBuilder.Entity<Role>()
        .HasMany(r => r.UserRoles)
        .WithOne(ur => ur.Role)
        .HasForeignKey(ur => ur.RoleId)
        .OnDelete(DeleteBehavior.Cascade);
    }
}