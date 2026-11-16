using System.ComponentModel.DataAnnotations.Schema;

namespace EventBooking.Api.Models;

[Table("SeatStatuses")]
public class SeatStatus
{
    [Column("seat_status_id")]
    public int SeatStatusId { get; set; }

    [Column("code")]
    public string Code { get; set; } = "";

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("is_active")]
    public bool IsActive { get; set; }
}