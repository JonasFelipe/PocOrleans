using Microsoft.AspNetCore.Mvc;
using PocOrleans.Application.Interfaces;
using PocOrleans.Domain.Entities;
using System;
using System.Threading.Tasks;

[Route("api/user")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserProfileService _service;

    public UserController(IUserProfileService service)
    {
        _service = service;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id) => Ok(await _service.GetProfileAsync(id));

    [HttpPost]
    public async Task<IActionResult> SetUser(Guid id, [FromBody] UserProfile profile)
    {
        await _service.SetProfileAsync(id, profile.Name, profile.Email);
        return Ok("User updated successfully");
    }
}
