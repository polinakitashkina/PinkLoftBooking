using Microsoft.EntityFrameworkCore;
using PinkLoftBooking.Api.Data;
using PinkLoftBooking.Api.Models.Domain;
namespace PinkLoftBooking.Api.Repositories;

public class BookingRepository : RepositoryBase, IBookingRepository
{
    public BookingRepository(AppDbContext context) : base(context) { }

    public Task<Booking?> GetByIdTrackedAsync(Guid id, CancellationToken ct = default) =>
        Context.Bookings.Include(b => b.Resource).FirstOrDefaultAsync(b => b.Id == id, ct);

    public async Task AddAsync(Booking booking, CancellationToken ct = default) =>
        await Context.Bookings.AddAsync(booking, ct);

    public Task<bool> HasActiveOverlapAsync(Guid resourceId, DateTime startUtc, DateTime endUtc, Guid? excludeBookingId, CancellationToken ct = default)
    {
        var q = Context.Bookings.AsNoTracking()
            .Where(b => b.ResourceId == resourceId && b.Status == BookingStatus.Active
                                              && b.StartUtc < endUtc && b.EndUtc > startUtc);
        if (excludeBookingId.HasValue)
            q = q.Where(b => b.Id != excludeBookingId.Value);
        return q.AnyAsync(ct);
    }

    public Task<int> CountActiveStartingOnUtcDayAsync(Guid userId, DateTime startUtc, Guid? excludeBookingId, CancellationToken ct = default)
    {
        var dayStart = DateTime.SpecifyKind(new DateTime(startUtc.Year, startUtc.Month, startUtc.Day), DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);
        var q = Context.Bookings.Where(
            b => b.UserId == userId
                 && b.Status == BookingStatus.Active
                 && b.StartUtc >= dayStart
                 && b.StartUtc < dayEnd);
        if (excludeBookingId.HasValue)
            q = q.Where(b => b.Id != excludeBookingId.Value);
        return q.CountAsync(ct);
    }

    public async Task<IReadOnlyList<Booking>> ListByUserAsync(Guid userId, CancellationToken ct = default) =>
        await Context.Bookings.AsNoTracking()
            .Include(b => b.Resource)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.StartUtc)
            .ToListAsync(ct);

    public Task SaveChangesAsync(CancellationToken ct = default) => Context.SaveChangesAsync(ct);
}
