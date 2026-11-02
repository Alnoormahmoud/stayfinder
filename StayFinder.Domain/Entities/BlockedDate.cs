namespace StayFinder.Domain.Entities;

// A range the host marked as unavailable (inclusive of both dates)
public class BlockedDate
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public DateOnly FromDate { get; set; }
    public DateOnly ToDate { get; set; }
    public string? Reason { get; set; }

    public Listing Listing { get; set; } = default!;
}
