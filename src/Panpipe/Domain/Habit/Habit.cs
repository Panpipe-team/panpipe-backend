using Ardalis.Result;
using Panpipe.Domain.HabitResult;

namespace Panpipe.Domain.Habit;

public class Habit
{
    private readonly List<HabitMark> marks = new ();

    public Guid Id { get; init; }
    public Guid ParamsSetId { get; init; }

    public ICollection<HabitMark> Marks => marks.AsReadOnly();

    public Result ChangeResult(Guid habitMarkId, AbstractHabitResult newResult) 
    {
        var habitMark = marks.FirstOrDefault(mark => mark.Id == habitMarkId);

        if (habitMark is null)
        {
            return Result.Invalid(new ValidationError(
                $"Habit with id {Id} does not have mark with id {habitMarkId}"
            ));
        }

        var firstMarkWithNonNullResult = marks.FirstOrDefault(
            mark => mark.Result is not null && mark.Id != habitMark.Id
        );

        if (firstMarkWithNonNullResult is not null && firstMarkWithNonNullResult.Result?.Type != newResult.Type)
        {
            return Result.Invalid(new ValidationError(
                "New habit result must be of the same type as everyone else in this habit, " +
                $"but in habit mark with id {firstMarkWithNonNullResult.Id} " +
                $"result type is {firstMarkWithNonNullResult.Result?.Type} and new result type is {newResult.Type}"
            ));
        }

        return habitMark.ChangeResult(newResult);
    }
}
