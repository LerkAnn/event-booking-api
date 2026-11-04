using EventBooking.Api.Data;
using EventBooking.Api.Dtos;
using EventBooking.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace EventBooking.Api.Controllers;
[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(RegisterRequest registerRequest)
    {
        if (await _db.Users.AnyAsync(u => u.Email == registerRequest.Email))
        {
            return Conflict("Email already registered.");
        }
        ;
        var user = new User
        {
            Name = registerRequest.Name,
            Email = registerRequest.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerRequest.Password),
            PhoneNumber = registerRequest.PhoneNumber,
            RoleId = 1,
        };
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return Ok(new { user.UserId, user.Name, user.Email });
    }
}
