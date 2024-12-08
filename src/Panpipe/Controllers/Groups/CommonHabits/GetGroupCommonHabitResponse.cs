using Panpipe.Controllers.Helpers;

namespace Panpipe.Controllers.Groups.CommonHabits;

public record GetGroupCommonHabitResponse(
    Guid Id,
    string Name, 
    string Description, 
    List<string> Tags, 
    Periodicity Periodicity, 
    string Goal, 
    string ResultType, 
    bool IsTemplated, 
    List<GetGroupCommonHabitResponseMark> Marks
);

public record GetGroupCommonHabitResponseMark(Guid Id, DateTime Timestamp, GetGroupCommonHabitResponseResult_? Result);

// Ended with underscore because of Swashbuckle schemaId intersection
public record GetGroupCommonHabitResponseResult_(string Value, string Comment);
