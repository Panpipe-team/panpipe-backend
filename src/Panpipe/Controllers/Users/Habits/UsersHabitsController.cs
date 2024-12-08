using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Controllers.Habits.Helpers;
using Panpipe.Persistence;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Users.Habits;

[ApiController]
[Route("/api/v1.1/users/habits")]
[Authorize]
public class UsersHabitsController(AppDbContext appDbContext, UserManager<AppIdentityUser> userManager): ControllerBase
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly UserManager<AppIdentityUser> _userManager = userManager;

    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<GetUserHabitsResponse>> GetAll()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        var result = await _appDbContext.UserHabitOwners
            .AsNoTracking()
            .Where(x => x.UserId == user.Id)
            .Join(
                _appDbContext.Habits
                    .AsNoTracking(),
                userHabitOwner => userHabitOwner.HabitId,
                habit => habit.Id,
                (userHabitOwner, habit) => new
                {
                    habit.Id,
                    habit.ParamsSetId
                }
            )
            .Join(
                _appDbContext.HabitParamsSets
                    .AsNoTracking()
                    .Include(x => x.Goal),
                habitIdAndHabitParamsSetId => habitIdAndHabitParamsSetId.ParamsSetId,
                paramsSet => paramsSet.Id,
                (habitIdAndHabitParamsSetId, paramsSet) => new {
                    habitIdAndHabitParamsSetId.Id,
                    paramsSet.Name,
                    paramsSet.Frequency,
                    paramsSet.Goal,
                    paramsSet.ResultType
                }
            )
            .ToListAsync();

        return Result.Success(new GetUserHabitsResponse(
            result.Select(habitInfo => new GetUserHabitsResponseHabit(
                habitInfo.Id,
                habitInfo.Name,
                Helpers.Periodicity.FromFrequency(habitInfo.Frequency),
                habitInfo.Goal.ToReadableString(),
                habitInfo.ResultType.ToString()
            )).ToList()
        ));
    }
    
    [HttpGet]
    [Route("{id:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetUserHabitByIdResponse>> GetById([FromRoute] Guid id)
    {
        const string ReplacementForNullComment = "";

        var result = await _appDbContext.Habits
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Include(x => x.Marks)
                .ThenInclude(x => x.Result)
            .Join(
                _appDbContext.HabitParamsSets
                    .AsNoTracking()
                    .Include(x => x.Goal)
                    .Include(x => x.Tags),
                habit => habit.ParamsSetId,
                paramsSet => paramsSet.Id,
                (habit, paramsSet) => new 
                {
                    paramsSet.Name,
                    paramsSet.Description,
                    paramsSet.Tags,
                    paramsSet.Frequency,
                    paramsSet.Goal,
                    paramsSet.ResultType,
                    paramsSet.IsPublicTemplate,
                    habit.Marks
                }
            )
            .FirstOrDefaultAsync();
        
        if (result is null)
        {
            return Result.NotFound();
        }
        
        return Result.Success(new GetUserHabitByIdResponse(
            result.Name,
            result.Description,
            result.Tags.Select(tag => tag.Name).ToList(),
            Helpers.Periodicity.FromFrequency(result.Frequency),
            result.Goal.ToReadableString(),
            result.ResultType.ToString(),
            result.IsPublicTemplate,
            result.Marks.Select(mark => new GetUserHabitByIdResponseMark(
                mark.Id, 
                mark.TimestampUtc.UtcDateTime,
                mark.Result is null 
                    ? null 
                    : new GetUserHabitByIdResponseMarkResult(
                        mark.Result.ToReadableString(), mark.Result.Comment ?? ReplacementForNullComment
                    ) 
            )).ToList()
        ));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<CreateUserHabitResponse>> Create([FromQuery] Guid? templateId)
    {
        // FAKED
        return Result.Success(new CreateUserHabitResponse(Guid.Empty));
    }

    [HttpGet]
    [Route("{habitId:guid}/statistics")]
    [TranslateResultToActionResult]
    public async Task<Result<GetUserHabitStatistics>> GetStatistics([FromRoute] Guid habitId)
    {
        // FAKED
        return Result.Success(new GetUserHabitStatistics(0.366f));
    }
}
