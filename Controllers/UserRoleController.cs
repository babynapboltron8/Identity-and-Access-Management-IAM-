using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IAM_API.Data;
using IAM_API.Entities;

namespace IAM_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IAMContext _context;

        public UserRoleController(IAMContext context)
        {
            _context = context;
        }
        

        // GET: api/UserRole
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserRole>>> GetUserRoles()
        {
            return await _context.UserRoles.ToListAsync();
        }

        //GET: api/UserRole/#
        [HttpGet("{id}")]
        public async Task<ActionResult<UserRole>> GetUserRole(Guid id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);  
            
            if (userRole == null)
            {
                return NotFound();
            }

            return userRole;
        }
    }
}