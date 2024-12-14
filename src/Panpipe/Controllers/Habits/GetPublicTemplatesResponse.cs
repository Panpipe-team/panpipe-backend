namespace Panpipe.Controllers.Habits;

public record GetPublicTemplatesResponse(List<GetPublicTemplatesResponseTemplate> Templates);

public record GetPublicTemplatesResponseTemplate(
    Guid TemplateId, string Name, string Periodicity, string Goal, string ResultType
);
