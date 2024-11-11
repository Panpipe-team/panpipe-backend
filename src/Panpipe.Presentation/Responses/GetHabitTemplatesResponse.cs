namespace Panpipe.Presentation.Responses;

public class GetHabitTemplatesResponse(List<GetHabitTemplatesResponseTemplate> templates): BaseResponse
{
    public List<GetHabitTemplatesResponseTemplate> Templates { get; } = templates;
}

public class GetHabitTemplatesResponseTemplate(
    Guid templateId, string name, string periodicity, string goal, string resultType
)
{
    public Guid TemplateId { get; } = templateId;
    public string Name { get; } = name;
    public string Periodicity { get; } = periodicity;
    public string Goal { get; } = goal;
    public string ResultType { get; } = resultType;
}
