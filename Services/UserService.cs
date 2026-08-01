using IAM_API.DTOs;
using IAM_API.Entities;
using IAM_API.Repositories;

namespace IAM_API.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();

        return users.Select(user => new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            IsLocked = user.IsLocked,
            CreatedAt = user.CreatedAt
        });
    }

    public async Task<UserResponseDto?> GetUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user == null)
        {
            return null;
        }

        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            IsLocked = user.IsLocked,
            CreatedAt = user.CreatedAt
        };
    }

    public async Task<(UserResponseDto? User, string? ErrorMessage)> CreateUserAsync(CreateUserDto dto)
    {
        if (await _userRepository.UsernameExistsAsync(dto.Username) || await _userRepository.EmailExistsAsync(dto.Email))
        {
            return (null, "Username or Email already exists.");
        }

        var distinctRoleIds = dto.RoleIds.Distinct().ToList();

        if (distinctRoleIds.Count > 0)
        {
            var existingRoleIds = await _userRepository.GetRoleIdsAsync(distinctRoleIds);
            var invalidRoleIds = distinctRoleIds.Except(existingRoleIds).ToList();

            if (invalidRoleIds.Count > 0)
            {
                return (null, $"Invalid role IDs: {string.Join(", ", invalidRoleIds)}");
            }
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email.Trim(),
            PasswordHash = dto.Password,
            IsActive = true,
            IsLocked = false,
            CreatedAt = DateTime.UtcNow
        };

        foreach (var roleId in distinctRoleIds)
        {
            user.UserRoles.Add(new UserRole
            {
                UserId = user.Id,
                RoleId = roleId,
                AssignedAt = DateTime.UtcNow
            });
        }

        await _userRepository.AddAsync(user);

        var response = new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            IsActive = user.IsActive,
            IsLocked = user.IsLocked,
            CreatedAt = user.CreatedAt
        };

        return (response, null);
    }

    public async Task<bool> UpdateUserAsync(Guid id, User user)
    {
        if (id != user.Id)
        {
            return false;
        }

        if (!await _userRepository.ExistsAsync(id))
        {
            return false;
        }

        await _userRepository.UpdateAsync(user);
        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        await _userRepository.DeleteAsync(user);
        return true;
    }
}
