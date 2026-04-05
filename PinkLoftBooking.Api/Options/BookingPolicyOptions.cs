namespace PinkLoftBooking.Api.Options;

public class BookingPolicyOptions
{
    public const string SectionName = "BookingPolicy";

    /// <summary>Максимум активных броней с началом в один календарный день (UTC) на пользователя.</summary>
    public int MaxBookingsPerDay { get; set; } = 5;
}
