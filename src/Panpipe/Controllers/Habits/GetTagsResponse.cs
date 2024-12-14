namespace Panpipe.Controllers.Habits;

public record GetTagsResponse(List<GetTagsResponseTag> Tags);

public record GetTagsResponseTag(Guid Id, string Name);
