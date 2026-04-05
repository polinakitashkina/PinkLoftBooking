using PinkLoftBooking.Api.Models.Domain;

namespace PinkLoftBooking.Api.Repositories;

public interface IBookingRepository
{
    Task<Booking?> GetByIdTrackedAsync(Guid id, CancellationToken ct = default);
    Task AddAsync(Booking booking, CancellationToken ct = default);
    Task<bool> HasActiveOverlapAsync(Guid resourceId, DateTime startUtc, DateTime endUtc, Guid? excludeBookingId, CancellationToken ct = default);
    Task<int> CountActiveStartingOnUtcDayAsync(Guid userId, DateTime startUtc, Guid? excludeBookingId, CancellationToken ct = default);
    Task<IReadOnlyList<Booking>> ListByUserAsync(Guid userId, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}
