using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IAM_API.Entities
{
    public class Role
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}