using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM_API.DTOs
{
    public class UserResponseDTO
    {
        public Guid Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public bool IsActive { get; set; }
    }
}