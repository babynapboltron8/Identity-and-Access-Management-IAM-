using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IAM_API.Services;
using Microsoft.AspNetCore.Mvc;
using IAM_API.DTOs;

namespace IAM_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.GetAllUsersAsync();

        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(CreateUserRequestDTO request)
    {
        var user = await _userService.CreateUserAsync(request);

        return CreatedAtAction(
            nameof(GetUserById),
            new { id = user.Id },
            user);
    }
}