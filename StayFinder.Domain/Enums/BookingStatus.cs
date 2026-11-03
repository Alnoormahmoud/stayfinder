namespace StayFinder.Domain.Enums;

public enum BookingStatus
{
    // Guest sent a request, waiting for the host
    Pending = 0,

    // Host accepted
    Confirmed = 1,

    // Host rejected the request
    Declined = 2,

    // Host did not answer before ExpiresAt
    Expired = 3,

    // Guest cancelled (while pending or confirmed)
    Cancelled = 4,

    // Stay is over (set by a background job after check-out)
    Completed = 5
}
