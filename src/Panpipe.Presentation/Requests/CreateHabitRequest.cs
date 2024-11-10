namespace Panpipe.Presentation.Requests;

public class CreateHabitRequest(Guid templateId): BaseRequest
{
    public Guid TemplateId { get; } = templateId;
}
