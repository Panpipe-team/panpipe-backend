namespace Panpipe.Presentation.Responses;

public class GetHabitsResponse(List<GetHabitsResponseHabit> habits) 
{
    public List<GetHabitsResponseHabit> Habits { get; } = habits;
}

public class GetHabitsResponseHabit(
    Guid habitId, string name, string periodicity, string goal, string resultType
): BaseResponse
{
    public Guid HabitId { get; } = habitId;
    public string Name { get; } = name;
    public string Periodicity { get; } = periodicity;
    public string Goal { get; } = goal;
    public string ResultType { get; } = resultType;
}
