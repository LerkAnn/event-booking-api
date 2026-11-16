using System.ComponentModel.DataAnnotations.Schema;

namespace EventBooking.Api.Models;

[Table("Bookings")]
public class Booking
{
    [Column("booking_id")]
    public int BookingId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("seat_id")]
    public int SeatId { get; set; }

    [Column("booking_status_id")]
    public int BookingStatusId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }
}