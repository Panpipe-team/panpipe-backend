using Panpipe.Controllers.Helpers;

namespace Panpipe.Controllers.Users.Habits;

public record GetUserHabitsResponse(List<GetUserHabitsResponseHabit> Habits);

public record GetUserHabitsResponseHabit(Guid Id, string Name, Periodicity Periodicity, string Goal, string ResultType);
