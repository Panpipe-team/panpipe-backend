using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Controllers.Helpers;
using Panpipe.Domain.Group;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Persistence;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Groups.CommonHabits;

[ApiController]
[Route("/api/v1.1/groups/{groupId:guid}/common-habits")]
[Authorize]
public class GroupCommonHabitsController(
    AppDbContext dbContext, UserManager<AppIdentityUser> userManager
) : ControllerBase
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly UserManager<AppIdentityUser> _userManager = userManager;

    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupCommonHabitsResponse>> GetAll([FromRoute] Guid groupId)
    {
        // FAKED
        return Result.Success(new GetGroupCommonHabitsResponse([
            new GetGroupCommonHabitsResponseHabit(
                Guid.Empty,
                "Faked group common habit name 1",
                Periodicity.FromFrequency(new Frequency(IntervalType.Day, 1)),
                "36.6",
                "Float"
            )
        ]));
    }

    [HttpGet]
    [Route("{habitId:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupCommonHabitResponse>> GetById([FromRoute] Guid groupId, [FromRoute] Guid habitId)
    {
        // FAKED
        return Result.Success(new GetGroupCommonHabitResponse(
            Guid.Empty,
            "Faked group common habit name 2",
            "Faked group common habit description 2",
            ["Faked tag 1", "Faked tag 2"],
            Periodicity.FromFrequency(new Frequency(IntervalType.Day, 1)),
            "36.6",
            "Float",
            false,
            [
                new GetGroupCommonHabitResponseMark(
                    Guid.Empty, 
                    DateTime.UtcNow, 
                    new GetGroupCommonHabitResponseResult_("38.5", "Лан, шучу, не болею")
                ),
                new GetGroupCommonHabitResponseMark(
                    Guid.Empty, 
                    DateTime.UtcNow, 
                    null
                )
            ]
        ));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<CreateGroupCommonHabitResponse>> Create(
        [FromRoute] Guid groupId,
        [FromQuery] Guid? templateId,
        [FromBody] CreateGroupCommonHabitRequest? request
    )
    {
        // FAKED
        return Result.Success(new CreateGroupCommonHabitResponse(Guid.Empty));
    }

    [HttpGet]
    [Route("{habitId:guid}/statistics")]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupCommonHabitStatisticsResponse>> GetStatistics(
        [FromRoute] Guid groupId, [FromRoute] Guid habitId
    )
    {
        // FAKED
        return Result.Success(new GetGroupCommonHabitStatisticsResponse(0.366f));
    }
}
