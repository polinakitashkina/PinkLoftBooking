namespace PinkLoftBooking.Api.Services;

/// <summary>Правила интервалов времени (используются в тестах и при необходимости в сервисах).</summary>
public static class BookingTimeRules
{
    /// <summary>Пересекаются ли два полуинтервала [aStart, aEnd) и [bStart, bEnd).</summary>
    public static bool IntervalsOverlap(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd)
    {
        if (aStart >= aEnd || bStart >= bEnd) return false;
        return aStart < bEnd && bStart < aEnd;
    }
}
