using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Controllers.Groups.PersonalHabits;
using Panpipe.Controllers.Helpers;
using Panpipe.Domain.Group;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Persistence;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Groups.PersonalHabits;

[ApiController]
[Route("/api/v1.1/groups/{groupId:guid}/personal-habits")]
[Authorize]
public class GroupPersonalHabitsController(
    AppDbContext dbContext, UserManager<AppIdentityUser> userManager
) : ControllerBase
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly UserManager<AppIdentityUser> _userManager = userManager;

    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupPersonalHabitsResponse>> GetAll([FromRoute] Guid groupId)
    {
        // FAKED
        return Result.Success(new GetGroupPersonalHabitsResponse([
            new GetGroupPersonalHabitsResponseHabit(
                Guid.Empty,
                "Faked group Personal habit name 3",
                Periodicity.FromFrequency(new Frequency(IntervalType.Week, 2)),
                "42",
                "Integer"
            )
        ]));
    }

    [HttpGet]
    [Route("{habitId:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupPersonalHabitResponse>> GetById([FromRoute] Guid groupId, [FromRoute] Guid habitId)
    {
        // FAKED
        return Result.Success(new GetGroupPersonalHabitResponse(
            "Faked group Personal habit name 4",
            "Faked group Personal habit description 4",
            ["Faked tag 3", "Faked tag 4"],
            Periodicity.FromFrequency(new Frequency(IntervalType.Week, 2)),
            "42",
            "Integer",
            false,
            [
                new GetGroupPersonalHabitResponseMark(
                    DateTime.UtcNow,
                    [
                        new GetGroupPersonalHabitResponsePersonalMark(
                            Guid.Empty,
                            Guid.Empty,
                            new GetGroupPersonalHabitResponseResult_("42", "Вселенная идеальна")
                        ),
                        new GetGroupPersonalHabitResponsePersonalMark(
                            Guid.Empty,
                            Guid.Empty,
                            null
                        )
                    ]
                )
                // 
            ]
        ));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<CreateGroupPersonalHabitResponse>> Create(
        [FromRoute] Guid groupId,
        [FromQuery] Guid? templateId,
        [FromBody] CreateGroupPersonalHabitRequest? request
    )
    {
        // FAKED
        return Result.Success(new CreateGroupPersonalHabitResponse(Guid.Empty));
    }

    [HttpGet]
    [Route("{habitId:guid}/statistics")]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupPersonalHabitStatisticsResponse>> GetStatistics(
        [FromRoute] Guid groupId, [FromRoute] Guid habitId
    )
    {
        // FAKED
        return Result.Success(new GetGroupPersonalHabitStatisticsResponse(0.42f));
    }
}
