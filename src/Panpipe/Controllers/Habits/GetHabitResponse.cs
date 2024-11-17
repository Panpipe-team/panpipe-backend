namespace Panpipe.Controllers.Habits;

public record GetHabitResponse(
    string Name, string Periodicity, string Goal, string ResultType, List<GetHabitResponseMark> Marks
);

public record GetHabitResponseMark(Guid Id, DateTime Timestamp, GetHabitResponseMarkResult? Result);

public record GetHabitResponseMarkResult(string Value);
