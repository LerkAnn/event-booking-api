using System.ComponentModel.DataAnnotations.Schema;

namespace EventBooking.Api.Models;

[Table("BookingStatuses")]
public class BookingStatus
{
    [Column("booking_status_id")]
    public int BookingStatusId { get; set; }

    [Column("code")]
    public string Code { get; set; } = "";

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("is_active")]
    public bool IsActive { get; set; }
}