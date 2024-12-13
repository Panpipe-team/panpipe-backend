using Ardalis.Result;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;

namespace Panpipe.Domain.Habit;

public class Habit(Guid id, Guid paramsSetId, HabitType habitType)
{
    private readonly List<HabitMark> marks = new ();

    public Guid Id { get; init; } = id;
    public Guid ParamsSetId { get; init; } = paramsSetId;
    public HabitType HabitType { get; init; } = habitType;

    public ICollection<HabitMark> Marks => marks.AsReadOnly();

    public Result AddEmptyMark(HabitMark habitMark)
    {
        if (habitMark.Result is not null)
        {
            return Result.Invalid(new ValidationError($"Habit mark with id {habitMark.Id} is not null"));
        }

        marks.Add(habitMark);

        return Result.Success();
    }

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
