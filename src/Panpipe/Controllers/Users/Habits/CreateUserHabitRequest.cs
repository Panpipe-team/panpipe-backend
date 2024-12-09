using Panpipe.Controllers.Helpers;

namespace Panpipe.Controllers.Users.Habits;

public record CreateUserHabitRequest(
    string Name, 
    string Description, 
    List<Guid> Tags, 
    Periodicity Periodicity, 
    string Goal, 
    string ResultType
);
