using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventBooking.Api.Models;

[Table("Roles")]
public class Role
{
    [Column("role_id")]
    public int RoleId { get; set; }

    [Required, StringLength(50)]
    [Column("code")]
    public string Code { get; set; } = "";

    [Required, StringLength(100)]
    [Column("name")]
    public string Name { get; set; } = "";

    [Column("is_active")]
    public bool IsActive { get; set; }
}