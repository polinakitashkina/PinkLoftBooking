namespace PinkLoftBooking.Api.Services;

/// <summary>Логические уведомления (без реальной отправки) — пишем в лог.</summary>
public interface INotificationService
{
    Task LogBookingEventAsync(string kind, Guid userId, Guid bookingId, CancellationToken ct = default);
}
