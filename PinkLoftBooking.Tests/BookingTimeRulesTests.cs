using PinkLoftBooking.Api.Services;
using Xunit;

namespace PinkLoftBooking.Tests;

public class BookingTimeRulesTests
{
    [Fact]
    public void IntervalsOverlap_TouchingEnd_NoOverlap()
    {
        var aStart = new DateTime(2026, 4, 5, 10, 0, 0, DateTimeKind.Utc);
        var aEnd = new DateTime(2026, 4, 5, 11, 0, 0, DateTimeKind.Utc);
        var bStart = aEnd;
        var bEnd = new DateTime(2026, 4, 5, 12, 0, 0, DateTimeKind.Utc);
        Assert.False(BookingTimeRules.IntervalsOverlap(aStart, aEnd, bStart, bEnd));
    }

    [Fact]
    public void IntervalsOverlap_Partial_ReturnsTrue()
    {
        var aStart = new DateTime(2026, 4, 5, 10, 0, 0, DateTimeKind.Utc);
        var aEnd = new DateTime(2026, 4, 5, 12, 0, 0, DateTimeKind.Utc);
        var bStart = new DateTime(2026, 4, 5, 11, 0, 0, DateTimeKind.Utc);
        var bEnd = new DateTime(2026, 4, 5, 13, 0, 0, DateTimeKind.Utc);
        Assert.True(BookingTimeRules.IntervalsOverlap(aStart, aEnd, bStart, bEnd));
    }

    [Fact]
    public void IntervalsOverlap_InvalidInterval_ReturnsFalse()
    {
        var t = new DateTime(2026, 4, 5, 10, 0, 0, DateTimeKind.Utc);
        Assert.False(BookingTimeRules.IntervalsOverlap(t, t, t, t.AddHours(1)));
    }
}
