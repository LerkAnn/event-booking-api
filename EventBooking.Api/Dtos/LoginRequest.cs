using System.ComponentModel.DataAnnotations;

namespace EventBooking.Api.Dtos;

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = "";

    [Required]
    public string Password { get; set; } = "";

}