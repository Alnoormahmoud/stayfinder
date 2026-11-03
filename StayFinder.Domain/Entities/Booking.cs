using StayFinder.Domain.Common;
using StayFinder.Domain.Enums;

namespace StayFinder.Domain.Entities;

public class Booking
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public Guid GuestId { get; set; }

    // Half-open stay: CheckIn is the first night, CheckOut is the departure day
    public DateOnly CheckIn { get; set; }
    public DateOnly CheckOut { get; set; }
    public int Guests { get; set; }

    // Price snapshot taken when the booking is made, so later price changes never alter it
    // Currency is copied from the listing (ISO 4217 code, for example USD, UGX, SDG)
    public string Currency { get; set; } = "USD";
    public decimal NightlyPrice { get; set; }
    public decimal CleaningFee { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal TotalPrice { get; set; }

    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Host approval flow
    public DateTime? ExpiresAt { get; set; }    // pending requests expire after this time
    public DateTime? RespondedAt { get; set; }  // when the host confirmed or declined
    public DateTime? CancelledAt { get; set; }
    public byte[] RowVersion { get; set; } = default!;

    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;
    public DateRange Stay => new(CheckIn, CheckOut);

    public Listing Listing { get; set; } = default!;
    public User Guest { get; set; } = default!;
    public Review? Review { get; set; }
}
