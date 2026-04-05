namespace PinkLoftBooking.Api.Services;

public class LogNotificationService(ILogger<LogNotificationService> logger) : INotificationService
{
    public Task LogBookingEventAsync(string kind, Guid userId, Guid bookingId, CancellationToken ct = default)
    {
        logger.LogInformation("Уведомление [{Kind}]: пользователь {UserId}, бронь {BookingId}", kind, userId, bookingId);
        return Task.CompletedTask;
    }
}
