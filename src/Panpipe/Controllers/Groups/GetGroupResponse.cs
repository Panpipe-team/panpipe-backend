namespace Panpipe.Controllers.Groups;

public record GetGroupResponse(string Name, List<GetGroupResponseParticipant> Participants);

public record GetGroupResponseParticipant(Guid UserId, string Name);
