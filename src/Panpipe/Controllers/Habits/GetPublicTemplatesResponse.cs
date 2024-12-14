using Panpipe.Controllers.Helpers;

namespace Panpipe.Controllers.Habits;

public record GetPublicTemplatesResponse(List<GetPublicTemplatesResponseTemplate> Templates);

public record GetPublicTemplatesResponseTemplate(
    Guid Id, 
    string Name, 
    string Description, 
    List<GetPublicTemplatesResponseTag> Tags, 
    Periodicity Periodicity, 
    string Goal, 
    string ResultType
);

public record GetPublicTemplatesResponseTag(Guid Id, string Name);
