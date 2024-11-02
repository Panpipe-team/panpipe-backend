namespace Panpipe.Domain.Entities;

public class Frequency
{
    public Frequency() { }

    public Frequency(
        IntervalType intervalType = IntervalType.Days,
        int intervalValue = 1)
    {
        IntervalType = intervalType;
        IntervalValue = intervalValue;
    }

    public IntervalType IntervalType { get; set; }
    public int IntervalValue { get; set; }
}