namespace StayFinder.Domain.Entities;

public class Amenity
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;

    // Icon name used by the React client (for example "wifi")
    public string Icon { get; set; } = default!;

    public ICollection<ListingAmenity> ListingAmenities { get; set; } = new List<ListingAmenity>();
}
