namespace Panpipe.Controllers.Groups;

public record GetGroupsResponse(List<GetGroupsResponseGroup> Groups);

public record GetGroupsResponseGroup(Guid GroupId, string Name);
