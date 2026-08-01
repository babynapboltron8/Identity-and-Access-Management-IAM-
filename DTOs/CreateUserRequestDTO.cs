using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM_API.DTOs
{
    public class CreateUserRequestDTO
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public Guid RoleId { get; set; }
    }
}