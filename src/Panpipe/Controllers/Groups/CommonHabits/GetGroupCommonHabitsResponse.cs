using Panpipe.Controllers.Helpers;

namespace Panpipe.Controllers.Groups.CommonHabits;

public record GetGroupCommonHabitsResponse(List<GetGroupCommonHabitsResponseHabit> Habits);

public record GetGroupCommonHabitsResponseHabit(
    Guid Id, 
    string Name, 
    Periodicity Periodicity, 
    string Goal, 
    string ResultType
);
