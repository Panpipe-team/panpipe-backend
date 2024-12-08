namespace Panpipe.Controllers.Groups;

public record CreateGroupRequest(string Name, List<CreateGroupRequestParticipant> Participants);

public record CreateGroupRequestParticipant(Guid UserId);
