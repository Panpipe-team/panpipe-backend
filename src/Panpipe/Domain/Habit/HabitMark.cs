using Ardalis.Result;
using Panpipe.Domain.HabitResult;

namespace Panpipe.Domain.Habit;

public class HabitMark
{
    private HabitMark() { }

    private HabitMark(Guid id, DateTimeOffset timestampUtc, AbstractHabitResult? result)
    {
        Id = id;
        TimestampUtc = timestampUtc;
        Result = result;
    }

    public Guid Id { get; init; }
    public DateTimeOffset TimestampUtc { get; init; }
    public AbstractHabitResult? Result { get; private set; }

    public static HabitMark CreateEmpty(Guid id, DateTimeOffset timestampUtc)
    {
        return new HabitMark(id, timestampUtc, null);
    }

    public Result ChangeResult(AbstractHabitResult newResult)
    {
        if (Result is AbstractHabitResult currentResult && currentResult.Type != newResult.Type)
        {
            return Ardalis.Result.Result.Invalid(new ValidationError(
                "New habit result must be of the same type as the previous one, " +
                $"but old is {currentResult.Type} and new is {newResult.Type}"
            ));
        }

        Result = newResult;

        return Ardalis.Result.Result.Success();
    }
}
