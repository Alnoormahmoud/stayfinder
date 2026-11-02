namespace StayFinder.Domain.Common;

/// <summary>
/// A stay as a half-open range: the check-in night is included, the check-out day is not.
/// That lets one guest leave on the 15th while another arrives on the 15th.
/// </summary>
public readonly record struct DateRange
{
    public DateOnly CheckIn { get; }
    public DateOnly CheckOut { get; }

    public DateRange(DateOnly checkIn, DateOnly checkOut)
    {
        if (checkOut <= checkIn)
            throw new ArgumentException("Check-out must be after check-in.");

        CheckIn = checkIn;
        CheckOut = checkOut;
    }

    public int Nights => CheckOut.DayNumber - CheckIn.DayNumber;

    public bool Overlaps(DateRange other) =>
        CheckIn < other.CheckOut && CheckOut > other.CheckIn;
}
