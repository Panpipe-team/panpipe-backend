using Panpipe.Domain.Entities.HabitResults;

namespace Panpipe.Presentation.Helpers;

public static class HabitResultExtensions
{
    public static string GetReadableValue(this AbstractHabitResult habitResult)
    {
        return habitResult switch 
        {
            HabitResultBoolean habitResultBoolean => habitResultBoolean.Value.ToString(),
            HabitResultFloat habitResultFloat => habitResultFloat.Value.ToString(),
            HabitResultInt habitResultInt => habitResultInt.Value.ToString(),
            HabitResultTime habitResultTime => habitResultTime.Value.ToString(),
            _ => throw new NotImplementedException()
        };
    }
}
