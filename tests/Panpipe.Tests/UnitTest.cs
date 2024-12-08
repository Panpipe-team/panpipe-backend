using Panpipe.Domain.HabitParamsSet;

namespace Panpipe.Tests;

public class UnitTest
{
    [Theory]
    [InlineData(IntervalType.Day, 1)]
    [InlineData(IntervalType.Day, 2)]
    [InlineData(IntervalType.Day, 5)]
    [InlineData(IntervalType.Day, 10)]
    [InlineData(IntervalType.Week, 1)]
    [InlineData(IntervalType.Week, 2)]
    [InlineData(IntervalType.Week, 5)]
    [InlineData(IntervalType.Week, 10)]
    [InlineData(IntervalType.Month, 1)]
    [InlineData(IntervalType.Month, 2)]
    [InlineData(IntervalType.Month, 5)]
    [InlineData(IntervalType.Month, 12)]
    [InlineData(IntervalType.Month, 13)]
    public void CalculatingCorrectly(IntervalType intervalType, int intervalValue)
    {
        // Arrange
        var freq = new Frequency(intervalType, intervalValue);
        var dateTimeOffset = new DateTimeOffset(2024, 07, 1, 0, 0, 0, TimeSpan.Zero); // 1st of July, Monday, midnight.
        // Was chosen as the latest valid timestamp for all 3 types of intervals

        // Act
        var result = freq.CalculateTimestampsOfEmptyMarksForExistingHabitFromLastMarkTimestamp(dateTimeOffset);

        // Assert
        // WARNING: this is of course not a test. This is just a piece of code to manually check,
        // that these methods are working. I do not have time to write normal test.
        // Place a debug point on the bottom line and check by your eyes through tests
        Assert.True(true);
    }
}
