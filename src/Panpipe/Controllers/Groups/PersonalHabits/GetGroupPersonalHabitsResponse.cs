using Panpipe.Controllers.Helpers;

namespace Panpipe.Controllers.Groups.PersonalHabits;

public record GetGroupPersonalHabitsResponse(List<GetGroupPersonalHabitsResponseHabit> Habits);

public record GetGroupPersonalHabitsResponseHabit(
    Guid Id,
    string Name,
    Periodicity Periodicity,
    string Goal,
    string ResultType
);
