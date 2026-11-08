using EventBooking.Api.Data;
using EventBooking.Api.Dtos;
using EventBooking.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;


namespace EventBooking.Api.Controllers;
[ApiController]
[Route("api/auth")]

public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _config;

    public AuthController(AppDbContext db, IConfiguration config)
    {
        _db = db;
        _config = config;
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

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(LoginRequest loginRequest)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

        if (user is null)
        {
            return Unauthorized("Invalid email or password.");
        }

        if (!BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash))
        {
            return Unauthorized("Invalid email or password.");
        }

        var roleCode = await _db.Roles.Where(r => r.RoleId == user.RoleId).Select(r => r.Code).FirstAsync();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, roleCode)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var jwt = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        var token = new JwtSecurityTokenHandler ().WriteToken(jwt);

        return Ok(new { token });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new { userId, email, role });
    }
}
