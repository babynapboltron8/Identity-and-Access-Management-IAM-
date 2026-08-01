using IAM_API.Data;
using IAM_API.Entities;
using Microsoft.EntityFrameworkCore;
using IAM_API.Repositories;
using IAM_API.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<IAMContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories and services (Dependency Injection)
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<IAMContext>();

    if (!context.Database.CanConnect())
    {
        context.Database.Migrate();
    }

    SeedRoles(context);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

static void SeedRoles(IAMContext context)
{
    var defaultRoles = new[]
    {
        new Role { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Admin", Description = "Administrator" },
        new Role { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "User", Description = "Regular user" }
    };

    foreach (var role in defaultRoles)
    {
        if (!context.Roles.Any(r => r.Name == role.Name))
        {
            context.Roles.Add(role);
        }
    }

    context.SaveChanges();
}
