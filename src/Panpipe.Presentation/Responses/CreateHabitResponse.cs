namespace Panpipe.Presentation.Responses;

public class CreateHabitResponse(Guid habitId): BaseResponse
{
    public Guid HabitId { get; } = habitId;
}
