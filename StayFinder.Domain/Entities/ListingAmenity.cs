namespace StayFinder.Domain.Entities;

public class ListingAmenity
{
    public Guid ListingId { get; set; }
    public int AmenityId { get; set; }

    public Listing Listing { get; set; } = default!;
    public Amenity Amenity { get; set; } = default!;
}
