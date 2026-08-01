using IAM_API.DTOs;
using IAM_API.Entities;

namespace IAM_API.Services;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetUsersAsync();
    Task<UserResponseDto?> GetUserAsync(Guid id);
    Task<(UserResponseDto? User, string? ErrorMessage)> CreateUserAsync(CreateUserDto dto);
    Task<bool> UpdateUserAsync(Guid id, User user);
    Task<bool> DeleteUserAsync(Guid id);
}
