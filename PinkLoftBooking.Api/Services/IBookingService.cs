using PinkLoftBooking.Api.DTOs.Bookings;

namespace PinkLoftBooking.Api.Services;

public interface IBookingService
{
    Task<(BookingResponse? r, string? error, int status)> CreateAsync(CreateBookingRequest dto, Guid userId, CancellationToken ct = default);
    Task<(BookingResponse? r, string? error, int status)> UpdateAsync(Guid bookingId, UpdateBookingRequest dto, Guid userId, bool isAdmin, CancellationToken ct = default);
    Task<(bool ok, string? error, int status)> CancelAsync(Guid bookingId, Guid userId, bool isAdmin, CancellationToken ct = default);
    Task<IReadOnlyList<BookingResponse>> ListMineAsync(Guid userId, CancellationToken ct = default);
}
