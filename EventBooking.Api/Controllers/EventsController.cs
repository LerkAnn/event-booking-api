using EventBooking.Api.Data;
using EventBooking.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventBooking.Api.Controllers;

[ApiController]
[Route("api/events")]
public class EventsController : ControllerBase
{
    private readonly AppDbContext _db;

    public EventsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.Events.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var ev = await _db.Events.FindAsync(id);
        return ev is null ? NotFound() : Ok(ev);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPost]
    public async Task<IActionResult> Create(Event ev)
    {
        _db.Events.Add(ev);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = ev.EventId }, ev);
    }

    [Authorize(Roles = "ADMIN")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev is null) return NotFound();
        _db.Events.Remove(ev);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    [Authorize(Roles = "ADMIN")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Event updated)
    {
        var ev = await _db.Events.FindAsync(id);
        if (ev is null) return NotFound();

        ev.Name = updated.Name;
        ev.StartTime = updated.StartTime;
        ev.TotalSeats = updated.TotalSeats;

        await _db.SaveChangesAsync();
        return NoContent();
    }
}