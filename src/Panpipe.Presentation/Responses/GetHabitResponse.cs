namespace Panpipe.Presentation.Responses;

public class GetHabitResponse(
    string name, 
    string periodicity, 
    string goal, 
    string resultType, 
    List<GetHabitResponseMark> marks
): BaseResponse
{
    public string Name { get; } = name;
    public string Periodicity { get; } = periodicity;
    public string Goal { get; } = goal;
    public string ResultType { get; } = resultType;
    public List<GetHabitResponseMark> Marks { get; } = marks;
}

public class GetHabitResponseMark(Guid id, DateTime timestamp, GetHabitResponseResult? result)
{
    public Guid Id { get; } = id;
    public DateTime Timestamp { get; } = timestamp;
    public GetHabitResponseResult? Result { get; } = result;
}

public class GetHabitResponseResult(string value)
{
    public string Value { get; } = value;
}
