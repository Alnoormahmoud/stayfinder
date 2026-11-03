using StayFinder.Domain.Enums;

namespace StayFinder.Domain.Common;

public static class BookingRules
{
    // Pending requests hold the dates, so two guests cannot request the same nights at once
    public static readonly BookingStatus[] DateBlockingStatuses =
    {
        BookingStatus.Pending,
        BookingStatus.Confirmed
    };

    // A pending request expires if the host does not answer in time
    public static readonly TimeSpan PendingRequestLifetime = TimeSpan.FromHours(48);
}
