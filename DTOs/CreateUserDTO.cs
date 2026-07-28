using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM_API.DTOs;

public class CreateUserDto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public List<Guid> RoleIds { get; set; } = new();
}