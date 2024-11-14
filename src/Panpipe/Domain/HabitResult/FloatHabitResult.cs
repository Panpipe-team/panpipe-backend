namespace Panpipe.Domain.HabitResult;

public class FloatHabitResult(Guid id, float value): AbstractHabitResult(id)
{
    public float Value { get; init; } = value;

    public override HabitResultType Type => HabitResultType.Float;
}
