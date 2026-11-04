using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EventBooking.Api.Models;
[Table("Users")]
public class User
{
    [Column("user_id")]
    public int UserId { get; set; }

    [Required, StringLength(200)]
    [Column("name")]
    public string Name { get; set; } = "";

    [Required, StringLength(200)]
    [Column("email")]
    public string Email { get; set; } = "";

    [Required, StringLength(200)]
    [Column("password_hash")]
    public string PasswordHash { get; set; } = "";

    [StringLength(20)]
    [Column("phone_number")]
    public string? PhoneNumber { get; set; }

    [Column("role_id")]
    public int RoleId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}
