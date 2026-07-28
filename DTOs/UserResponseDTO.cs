using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM_API.DTOs
{
    public class UserResponseDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public bool IsActive { get; set; }
    public bool IsLocked { get; set; }
    public DateTime CreatedAt { get; set; }
}
}