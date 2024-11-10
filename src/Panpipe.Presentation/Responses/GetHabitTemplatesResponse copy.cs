namespace Panpipe.Presentation.Responses;

public class GetHabitTemplatesResponse(
    Guid templateId, string name, string periodicity, string goal, string resultType
): BaseResponse
{
    public Guid TemplateId { get; } = templateId;
    public string Name { get; } = name;
    public string Periodicity { get; } = periodicity;
    public string Goal { get; } = goal;
    public string ResultType { get; } = resultType;
}
