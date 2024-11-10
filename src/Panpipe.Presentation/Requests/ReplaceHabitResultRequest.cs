namespace Panpipe.Presentation.Requests;

public class ReplaceHabitResultRequest(string value)
{
    public string Value { get; } = value;
}
