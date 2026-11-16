using EventBooking.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace EventBooking.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<SeatStatus> SeatStatuses { get; set; }
    public DbSet<BookingStatus> BookingStatuses { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>().Property(b => b.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
        modelBuilder.Entity<Booking>().Property(b => b.UpdatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
    }
}