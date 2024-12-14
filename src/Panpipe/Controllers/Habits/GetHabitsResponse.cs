namespace Panpipe.Controllers.Habits;

public record GetHabitsResponse(List<GetHabitsResponseHabit> Habits);

public record GetHabitsResponseHabit(Guid HabitId, string Name, string Periodicity, string Goal, string ResultType);
