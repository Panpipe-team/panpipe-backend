using Panpipe.Controllers.Helpers;

namespace Panpipe.Controllers.Users.Habits;

public record GetUserHabitByIdResponse(
    string Name, 
    string Description, 
    List<string> Tags, 
    Periodicity Periodicity, 
    string Goal, 
    string ResultType, 
    bool IsTemplated, 
    List<GetUserHabitByIdResponseMark> Marks
);

public record GetUserHabitByIdResponseMark(Guid Id, DateTime Timestamp, GetUserHabitByIdResponseMarkResult? Result);

public record GetUserHabitByIdResponseMarkResult(string Value, string Comment);
