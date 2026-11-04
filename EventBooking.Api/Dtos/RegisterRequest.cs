using System.ComponentModel.DataAnnotations;

namespace EventBooking.Api.Dtos;

public class RegisterRequest
{
    [Required, StringLength(200)]
    public string Name { get; set; } = "";

    [Required, StringLength(200)]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required, StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = "";

    [StringLength(20)]
    public string? PhoneNumber { get; set; }
}
