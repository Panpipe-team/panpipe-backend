using Panpipe.Controllers.Helpers;

namespace Panpipe.Controllers.Groups.CommonHabits;

public record CreateGroupCommonHabitRequest(
    string Name, 
    string Description, 
    List<Guid> Tags, 
    Periodicity Periodicity, 
    string Goal, 
    string ResultType
);

