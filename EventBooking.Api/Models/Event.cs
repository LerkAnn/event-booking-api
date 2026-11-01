using System.ComponentModel.DataAnnotations.Schema;

namespace EventBooking.Api.Models;

[Table("Events")]
public class Event
{
    [Column("event_id")]
    public int EventId { get; set; }

    [Column("name")]
    public string Name { get; set; } = "";

    [Column("start_time")]
    public DateTime StartTime { get; set; }

    [Column("total_seats")]
    public int TotalSeats { get; set; }
}