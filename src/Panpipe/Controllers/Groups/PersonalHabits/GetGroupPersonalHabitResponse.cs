using Panpipe.Controllers.Helpers;

namespace Panpipe.Controllers.Groups.PersonalHabits;

public record GetGroupPersonalHabitResponse(
    string Name,
    string Description,
    List<string> Tags,
    Periodicity Periodicity,
    string Goal,
    string ResultType,
    bool IsTemplated,
    List<GetGroupPersonalHabitResponseMark> Marks
);

public record GetGroupPersonalHabitResponseMark(
    DateTime Timestamp, List<GetGroupPersonalHabitResponsePersonalMark> PersonalMarks
);

public record GetGroupPersonalHabitResponsePersonalMark(
    Guid Id, Guid UserId, GetGroupPersonalHabitResponseResult_? Result
);

// Ended with underscore because of Swashbuckle schemaId intersection
public record GetGroupPersonalHabitResponseResult_(string Value, string Comment);
