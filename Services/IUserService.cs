using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAM_API.Entities;
using IAM_API.DTOs;

namespace IAM_API.Services
{
    public interface IUserService
    {
      
      Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();

      Task<UserResponseDTO?> GetUserByIdAsync(Guid id);

      Task<UserResponseDTO> CreateUserAsync(CreateUserRequestDTO request);

    }
}