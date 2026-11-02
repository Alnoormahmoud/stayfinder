namespace StayFinder.Domain.Entities;

public class ListingImage
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public string Url { get; set; } = default!;
    public bool IsCover { get; set; }
    public int SortOrder { get; set; }

    public Listing Listing { get; set; } = default!;
}
