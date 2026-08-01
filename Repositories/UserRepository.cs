using IAM_API.Data;
using IAM_API.Entities;
using Microsoft.EntityFrameworkCore;

namespace IAM_API.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IAMContext _context;

    public UserRepository(IAMContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}