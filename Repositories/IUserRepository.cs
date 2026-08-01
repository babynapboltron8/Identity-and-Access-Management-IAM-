using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAM_API.Entities;

namespace IAM_API.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(Guid id);
        Task AddAsync(User user);
        Task SaveChangesAsync();
    }
}