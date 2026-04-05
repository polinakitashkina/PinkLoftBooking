using Microsoft.Extensions.Options;
using PinkLoftBooking.Api.DTOs.Bookings;
using PinkLoftBooking.Api.Models.Domain;
using PinkLoftBooking.Api.Options;
using PinkLoftBooking.Api.Repositories;

namespace PinkLoftBooking.Api.Services;

public class BookingService(
    IBookingRepository bookings,
    IResourceRepository resources,
    IOptions<BookingPolicyOptions> policy,
    INotificationService notifications,
    ILogger<BookingService> logger) : IBookingService
{
    private readonly int _maxPerDay = Math.Max(1, policy.Value.MaxBookingsPerDay);

    public async Task<(BookingResponse? r, string? error, int status)> CreateAsync(CreateBookingRequest dto, Guid userId, CancellationToken ct = default)
    {
        var resource = await resources.GetByIdAsync(dto.ResourceId, ct);
        if (resource is null)
            return (null, "Такой зоны нет ✨", 404);

        var start = EnsureUtc(dto.StartUtc);
        var end = EnsureUtc(dto.EndUtc);
        var validation = ValidateInterval(start, end);
        if (validation is not null) return (null, validation, 400);

        if (await bookings.HasActiveOverlapAsync(dto.ResourceId, start, end, null, ct))
            return (null, "Это время уже занято — выбери другой слот 💗", 409);

        var count = await bookings.CountActiveStartingOnUtcDayAsync(userId, start, null, ct);
        if (count >= _maxPerDay)
            return (null, $"Максимум {_maxPerDay} активных броней на один календарный день.", 400);

        var booking = new Booking
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ResourceId = dto.ResourceId,
            StartUtc = start,
            EndUtc = end,
            Status = BookingStatus.Active
        };
        await bookings.AddAsync(booking, ct);
        await bookings.SaveChangesAsync(ct);
        logger.LogInformation("Создана бронь {BookingId} ресурс {ResourceId}", booking.Id, dto.ResourceId);
        await notifications.LogBookingEventAsync("создана", userId, booking.Id, ct);

        return (Map(booking, resource.Name), null, 201);
    }

    public async Task<(BookingResponse? r, string? error, int status)> UpdateAsync(Guid bookingId, UpdateBookingRequest dto, Guid userId, bool isAdmin, CancellationToken ct = default)
    {
        var booking = await bookings.GetByIdTrackedAsync(bookingId, ct);
        if (booking is null) return (null, "Бронь не найдена", 404);
        if (!isAdmin && booking.UserId != userId) return (null, "Нельзя менять чужую бронь", 403);
        if (booking.Status != BookingStatus.Active) return (null, "Отменённую бронь нельзя перенести", 400);

        var start = EnsureUtc(dto.StartUtc);
        var end = EnsureUtc(dto.EndUtc);
        var validation = ValidateInterval(start, end);
        if (validation is not null) return (null, validation, 400);

        if (await bookings.HasActiveOverlapAsync(booking.ResourceId, start, end, bookingId, ct))
            return (null, "Пересечение с другой активной бронью", 409);

        var countOnNewDay = await bookings.CountActiveStartingOnUtcDayAsync(booking.UserId, start, bookingId, ct);
        if (countOnNewDay >= _maxPerDay)
            return (null, $"Максимум {_maxPerDay} активных броней на один календарный день.", 400);

        booking.StartUtc = start;
        booking.EndUtc = end;
        await bookings.SaveChangesAsync(ct);
        logger.LogInformation("Обновлена бронь {BookingId}", bookingId);
        await notifications.LogBookingEventAsync("изменена", userId, bookingId, ct);

        if (booking.Resource is null)
            booking.Resource = (await resources.GetByIdAsync(booking.ResourceId, ct))!;
        return (Map(booking), null, 200);
    }

    public async Task<(bool ok, string? error, int status)> CancelAsync(Guid bookingId, Guid userId, bool isAdmin, CancellationToken ct = default)
    {
        var booking = await bookings.GetByIdTrackedAsync(bookingId, ct);
        if (booking is null) return (false, "Бронь не найдена", 404);
        if (!isAdmin && booking.UserId != userId) return (false, "Нельзя отменить чужую бронь", 403);
        if (booking.Status == BookingStatus.Cancelled) return (false, "Бронь уже отменена", 400);

        booking.Status = BookingStatus.Cancelled;
        await bookings.SaveChangesAsync(ct);
        logger.LogInformation("Отменена бронь {BookingId}", bookingId);
        await notifications.LogBookingEventAsync("отменена", userId, bookingId, ct);
        return (true, null, 200);
    }

    public async Task<IReadOnlyList<BookingResponse>> ListMineAsync(Guid userId, CancellationToken ct = default)
    {
        var list = await bookings.ListByUserAsync(userId, ct);
        return list.Select(b => Map(b)).ToList();
    }

    private static BookingResponse Map(Booking b, string? resourceName = null) => new()
    {
        Id = b.Id,
        ResourceId = b.ResourceId,
        ResourceName = resourceName ?? b.Resource?.Name ?? "",
        StartUtc = b.StartUtc,
        EndUtc = b.EndUtc,
        Status = b.Status
    };

    private static string? ValidateInterval(DateTime start, DateTime end)
    {
        if (end <= start) return "Время окончания должно быть позже начала";
        return null;
    }

    private static DateTime EnsureUtc(DateTime dt) => dt.Kind switch
    {
        DateTimeKind.Utc => dt,
        DateTimeKind.Local => dt.ToUniversalTime(),
        _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
    };
}
