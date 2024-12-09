using Ardalis.Result;
using Panpipe.Domain.HabitResult;

namespace Panpipe.Controllers.Helpers;

public static class HabitResultTypeParser
{
    public static Result<HabitResultType> Parse(string s)
    {
        HabitResultType? result = s switch
        {
            "Boolean" => HabitResultType.Boolean,
            "Float" => HabitResultType.Float,
            _ => null,
        };

        if (result is null)
        {
            return Result.Invalid(new ValidationError($"Invalid habit result type \"{s}\""));
        }

        return Result.Success(result.Value);
    }
}
