using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PinkLoftBooking.Api.DTOs.Bookings;
using PinkLoftBooking.Api.Models.Domain;
using PinkLoftBooking.Api.Services;

namespace PinkLoftBooking.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BookingsController(IBookingService bookings) : ControllerBase
{
    [HttpGet("my")]
    [ProducesResponseType(typeof(IReadOnlyList<BookingResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> My(CancellationToken ct)
    {
        var list = await bookings.ListMineAsync(UserId(), ct);
        return Ok(list);
    }

    [HttpPost]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateBookingRequest dto, CancellationToken ct)
    {
        var (r, error, status) = await bookings.CreateAsync(dto, UserId(), ct);
        return ToActionResult(r, error, status);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(BookingResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookingRequest dto, CancellationToken ct)
    {
        var (r, error, status) = await bookings.UpdateAsync(id, dto, UserId(), IsAdmin(), ct);
        return ToActionResult(r, error, status);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel(Guid id, CancellationToken ct)
    {
        var (ok, error, status) = await bookings.CancelAsync(id, UserId(), IsAdmin(), ct);
        if (!ok && error is not null)
            return StatusCode(status, new { message = error });
        return NoContent();
    }

    private IActionResult ToActionResult(BookingResponse? r, string? error, int status)
    {
        if (error is not null)
            return StatusCode(status, new { message = error });
        if (status == 201 && r is not null)
            return Created($"/api/bookings/{r.Id}", r);
        return Ok(r);
    }

    private Guid UserId() => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private bool IsAdmin() => User.IsInRole(nameof(UserRole.Admin));
}
