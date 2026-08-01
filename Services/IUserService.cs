using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAM_API.Entities;

namespace IAM_API.Services
{
    public interface IUserService
    {
      
    Task<IEnumerable<User>> GetAllUsersAsync();

    }
}