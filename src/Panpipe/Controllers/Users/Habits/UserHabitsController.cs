using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Controllers.Helpers;
using Panpipe.Domain.Habit;
using Panpipe.Domain.HabitOwner;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;
using Panpipe.Persistence;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Users.Habits;

[ApiController]
[Route("/api/v1.1/users/habits")]
[Authorize]
public class UserHabitsController(AppDbContext appDbContext, UserManager<AppIdentityUser> userManager): ControllerBase
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
                Periodicity.FromFrequency(habitInfo.Frequency),
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
            Periodicity.FromFrequency(result.Frequency),
            result.Goal.ToReadableString(),
            result.ResultType.ToString(),
            result.IsPublicTemplate,
            result.Marks.Select(mark => new GetUserHabitByIdResponseMark(
                mark.Id, 
                mark.TimestampUtc.UtcDateTime,
                mark.Result is null 
                    ? null 
                    : new GetUserHabitByIdResponseMarkResult(mark.Result.ToReadableString(), mark.Result.Comment) 
            )).ToList()
        ));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<CreateUserHabitResponse>> Create(
        [FromQuery] Guid? templateId, [FromBody] CreateUserHabitRequest? request
    )
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        Guid habitParamsSetId;

        if (request is not null)
        {
            if (templateId is not null)  
            {
                return Result.Invalid(new ValidationError(
                    "TemplateId from query and habit parameters in non-null body cannot be specified simultaneously"
                ));
            }

            var tags = await _appDbContext.Tags
                .AsNoTracking()
                .Where(tag => request.Tags.Contains(tag.Id))
                .ToListAsync();

            var habitResultTypeResult = HabitResultTypeParser.Parse(request.ResultType);

            if (habitResultTypeResult.IsInvalid())
            {
                return Result.Invalid(habitResultTypeResult.ValidationErrors);
            }

            var habitResultType = habitResultTypeResult.Value;

            string? goalComment = null;

            var goalParsedSuccessfully = habitResultType.TryParse(request.Goal, goalComment, out var goal);

            if (!goalParsedSuccessfully)
            {
                return Result.Invalid(new ValidationError($"Goal \"{request.Goal}\" cannot be parsed"));
            }

            var frequencyResult = request.Periodicity.ToFrequency();

            if (frequencyResult.IsInvalid())
            {
                return Result.Invalid(frequencyResult.ValidationErrors);
            }

            var frequency = frequencyResult.Value;

            const bool isPublicTemplate = false;

            var newHabitParamsSet = new HabitParamsSet(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                tags,
                goal,
                frequency,
                isPublicTemplate
            );

            _appDbContext.HabitParamsSets.Add(newHabitParamsSet);

            await _appDbContext.SaveChangesAsync();

            habitParamsSetId = newHabitParamsSet.Id;
        }
        else {
            if (templateId is null)
            {
                return Result.Invalid(new ValidationError(
                    "Exactly one of templateId from query and habit parameters in non-null body must be specified, " +
                    "not none of them"
                ));
            }

            habitParamsSetId = templateId.Value;
        }

        var habitParamsSet = await _appDbContext.HabitParamsSets
            .AsNoTracking()
            .Where(x => x.Id == habitParamsSetId)
            .FirstOrDefaultAsync();

        if (habitParamsSet is null)
        {
            return Result.Invalid(new ValidationError($"Not found habit params template with id {habitParamsSetId}"));
        }

        var habit = new Habit(Guid.NewGuid(), habitParamsSetId);

        _appDbContext.Habits.Add(habit);

        var userHabitOwner = new UserHabitOwner(Guid.NewGuid(), user.Id, habit.Id);

        _appDbContext.UserHabitOwners.Add(userHabitOwner);

        var emptyMarksTimestamps = habitParamsSet.CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

        foreach (var timestamp in emptyMarksTimestamps)
        {
            var emptyMark = HabitMark.CreateEmpty(Guid.NewGuid(), timestamp);
            habit.AddEmptyMark(emptyMark);
        }

        await _appDbContext.SaveChangesAsync();

        return Result.Created(new CreateUserHabitResponse(habit.Id));
    }

    [HttpGet]
    [Route("{habitId:guid}/statistics")]
    [TranslateResultToActionResult]
    public async Task<Result<GetUserHabitStatistics>> GetStatistics([FromRoute] Guid habitId)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        var currentUserHabit = await _appDbContext.Habits
            .AsNoTracking()
            .Include(habit => habit.Marks)
                .ThenInclude(habitMark => habitMark.Result)
            .Where(habit => habit.Id == habitId)
            .FirstOrDefaultAsync();
        
        if (currentUserHabit is null)
        {
            return Result.NotFound($"Habit with id {habitId} was not found");
        }

        var habitParamsSet = await _appDbContext.HabitParamsSets
            .AsNoTracking()
            .Include(habitParamsSet => habitParamsSet.Goal)
            .Where(habitParamsSet => habitParamsSet.Id == currentUserHabit.ParamsSetId)
            .FirstOrDefaultAsync();
        
        if (habitParamsSet is null)
        {
            return Result.CriticalError($"Habit params set with id {currentUserHabit.ParamsSetId} cannot be found");
        }

        var allHabitsExceptCurrentWithSameParams = await _appDbContext.Habits
            .AsNoTracking()
            .Include(habit => habit.Marks)
                .ThenInclude(habitMark => habitMark.Result)
            .Where(habit => habit.ParamsSetId == habitParamsSet.Id && habit.Id != habitId)
            .ToListAsync();
        
        var goal = habitParamsSet.Goal;

        var currentUserStreak = currentUserHabit.GetStreak(goal);

        if (currentUserStreak == 0)
        {
            return Result.Success(new GetUserHabitStatistics(0));
        }
        
        var otherUsersStreaks = allHabitsExceptCurrentWithSameParams.Select(habit => habit.GetStreak(goal)).ToList();

        if (otherUsersStreaks.Count == 0)
        {
            const int betterThanEverybodyAsThereIsNobody = 1;

            return Result.Success(new GetUserHabitStatistics(betterThanEverybodyAsThereIsNobody));
        }

        var countCurrentUserBetterStreak = otherUsersStreaks.Count(otherStreak => currentUserStreak >= otherStreak);

        var currentUserBetterStreakProportion = (float) (countCurrentUserBetterStreak + 1) / (otherUsersStreaks.Count + 1);

        return Result.Success(new GetUserHabitStatistics(currentUserBetterStreakProportion));
    }
}
