using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Controllers.Helpers;
using Panpipe.Domain.Habit;
using Panpipe.Domain.HabitCollection;
using Panpipe.Domain.HabitOwner;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;
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
    private readonly AppDbContext _appDbContext = dbContext;
    private readonly UserManager<AppIdentityUser> _userManager = userManager;

    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupPersonalHabitsResponse>> GetAll([FromRoute] Guid groupId)
    {
        var result = await _appDbContext.HabitCollections
            .AsNoTracking()
            .Where(habitCollection => habitCollection.GroupId == groupId)
            .Join(
                _appDbContext.HabitParamsSets
                    .AsNoTracking()
                    .Include(x => x.Goal),
                habitCollection => habitCollection.ParamsSetId,
                paramsSet => paramsSet.Id,
                (habitCollection, paramsSet) => new {
                    habitCollection.Id,
                    paramsSet.Name,
                    paramsSet.Frequency,
                    paramsSet.Goal,
                    paramsSet.ResultType
                }
            )
            .ToListAsync();

        return Result.Success(new GetGroupPersonalHabitsResponse(
            result.Select(habitInfo => new GetGroupPersonalHabitsResponseHabit(
                habitInfo.Id,
                habitInfo.Name,
                Periodicity.FromFrequency(habitInfo.Frequency),
                habitInfo.Goal.ToReadableString(),
                habitInfo.ResultType.ToString()
            )).ToList()
        ));
    }

    [HttpGet]
    [Route("{habitId:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupPersonalHabitResponse>> GetById([FromRoute] Guid groupId, [FromRoute] Guid habitId)
    {
        var result = await _appDbContext.HabitCollections
            .AsNoTracking()
            .Where(x => x.Id == habitId)
            .Join(
                _appDbContext.HabitParamsSets
                    .AsNoTracking()
                    .Include(x => x.Goal)
                    .Include(x => x.Tags),
                habit => habit.ParamsSetId,
                paramsSet => paramsSet.Id,
                (habit, paramsSet) => new 
                {
                    habit.HabitIds,
                    paramsSet.Name,
                    paramsSet.Description,
                    paramsSet.Tags,
                    paramsSet.Frequency,
                    paramsSet.Goal,
                    paramsSet.ResultType,
                    paramsSet.IsPublicTemplate,
                }
            )
            .FirstOrDefaultAsync();
        
        if (result is null)
        {
            return Result.NotFound();
        }

        var subHabitResults = await _appDbContext.Habits
            .AsNoTracking()
            .Include(habit => habit.Marks)
                .ThenInclude(habitMark => habitMark.Result)
            .Where(habit => result.HabitIds.Contains(habit.Id))
            .Join(
                _appDbContext.GroupUserHabitOwners
                    .AsNoTracking(),
                habit => habit.Id,
                groupUserHabitOwner => groupUserHabitOwner.HabitId,
                (habit, habitOwner) => new
                {
                    habit,
                    habitOwner.UserId
                }
            )
            .ToListAsync();
        
        var marksDictionary = new Dictionary<DateTimeOffset, List<GetGroupPersonalHabitResponsePersonalMark>> ();

        foreach (var subHabitResult in subHabitResults)
        {
            var subHabit = subHabitResult.habit;
            var userId = subHabitResult.UserId;

            foreach (var mark in subHabit.Marks)
            {
                var timestamp = mark.TimestampUtc;

                if (!marksDictionary.TryGetValue(timestamp, out var currentList))
                {
                    currentList = [];
                    marksDictionary.Add(timestamp, currentList);
                }

                currentList.Add(
                    new GetGroupPersonalHabitResponsePersonalMark(
                        mark.Id,
                        userId,
                        mark.Result is null 
                            ? null
                            : new GetGroupPersonalHabitResponseResult_(
                                mark.Result.ToReadableString(),
                                mark.Result.Comment
                            )
                    )
                );
            }
        }
        
        return Result.Success(new GetGroupPersonalHabitResponse(
            result.Name,
            result.Description,
            result.Tags.Select(tag => tag.Name).ToList(),
            Periodicity.FromFrequency(result.Frequency),
            result.Goal.ToReadableString(),
            result.ResultType.ToString(),
            result.IsPublicTemplate,
            marksDictionary
                .OrderBy(timestampAndMarksPair => timestampAndMarksPair.Key.UtcDateTime)
                .Select(timestampAndMarksPair => new GetGroupPersonalHabitResponseMark(
                    timestampAndMarksPair.Key.UtcDateTime,
                    timestampAndMarksPair.Value
                )).ToList()
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

            if (request.Name == "")
            {
                return Result.Invalid(new ValidationError("Habit name cannot be an empty string"));
            }

            var tags = await _appDbContext.Tags
                .Where(tag => request.Tags.Contains(tag.Id))
                .ToListAsync();

            var habitResultTypeResult = HabitResultTypeParser.Parse(request.ResultType);

            if (habitResultTypeResult.IsInvalid())
            {
                return Result.Invalid(habitResultTypeResult.ValidationErrors);
            }

            var habitResultType = habitResultTypeResult.Value;

            const string GoalComment = "";

            var goalParsedSuccessfully = habitResultType.TryParse(request.Goal, GoalComment, out var goal);

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

        var group = await _appDbContext.Groups
            .AsNoTracking()
            .Where(group => group.Id == groupId)
            .FirstOrDefaultAsync();
        
        if (group is null)
        {
            return Result.Invalid(new ValidationError($"Not found group with id {groupId}"));
        }

        var habitIds = new List<Guid> ();

        foreach (var participantId in group.UserIds)
        {
            var habit = new Habit(Guid.NewGuid(), habitParamsSetId);

            _appDbContext.Habits.Add(habit);

            habitIds.Add(habit.Id);

            var habitOwner = new GroupUserHabitOwner(Guid.NewGuid(), participantId, groupId, habit.Id);

            _appDbContext.GroupUserHabitOwners.Add(habitOwner);

            var emptyMarksTimestamps = habitParamsSet.CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

            foreach (var timestamp in emptyMarksTimestamps)
            {
                var emptyMark = HabitMark.CreateEmpty(Guid.NewGuid(), timestamp);
                habit.AddEmptyMark(emptyMark);
            }
        }

        var habitCollection = new HabitCollection(Guid.NewGuid(), habitParamsSetId, groupId, habitIds);

        _appDbContext.HabitCollections.Add(habitCollection);
        
        await _appDbContext.SaveChangesAsync();

        return Result.Success(new CreateGroupPersonalHabitResponse(habitCollection.Id));
    }

    [HttpGet]
    [Route("{habitId:guid}/statistics")]
    [TranslateResultToActionResult]
    public async Task<Result<GetGroupPersonalHabitStatisticsResponse>> GetStatistics(
        [FromRoute] Guid groupId, [FromRoute] Guid habitId
    )
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        var habitCollection = await _appDbContext.HabitCollections
            .AsNoTracking()
            .Where(habitCollection => habitCollection.Id == habitId)
            .FirstOrDefaultAsync();
        
        if (habitCollection is null)
        {
            return Result.NotFound($"Habit with id {habitId} was not found");
        }

        var allHabits = await _appDbContext.Habits
            .AsNoTracking()
            .Include(habit => habit.Marks)
                .ThenInclude(habitMark => habitMark.Result)
            .Where(habit => habitCollection.HabitIds.Contains(habit.Id))
            .Join(
                _appDbContext.GroupUserHabitOwners
                    .AsNoTracking(),
                habit => habit.Id,
                habitOwner => habitOwner.HabitId,
                (habit, habitOwner) => new {
                    habit,
                    habitOwner.UserId
                }
            )
            .ToListAsync();

        var currentHabit = allHabits
            .Where(habitAndHabitOwner => habitAndHabitOwner.UserId == user.Id)
            .Select(habitAndHabitOwner => habitAndHabitOwner.habit)
            .FirstOrDefault();
        
        if (currentHabit is null)
        {
            return Result.CriticalError(
                $"Cannot find personal habit for user with id {user.Id} within habit collection with id {habitId}"
            );
        }

        var otherHabits = allHabits
            .Where(habitAndHabitOwner => habitAndHabitOwner.UserId != user.Id)
            .Select(habitAndHabitOwner => habitAndHabitOwner.habit)
            .ToList();

        var habitParamsSet = await _appDbContext.HabitParamsSets
            .AsNoTracking()
            .Include(habitParamsSet => habitParamsSet.Goal)
            .Where(habitParamsSet => habitParamsSet.Id == habitCollection.ParamsSetId)
            .FirstOrDefaultAsync();
        
        if (habitParamsSet is null)
        {
            return Result.CriticalError($"Habit params set with id {currentHabit.ParamsSetId} cannot be found");
        }
        
        var goal = habitParamsSet.Goal;

        var result = CommonOperations.CalculateStatistics(currentHabit, otherHabits, goal);

        return result.Map(value => new GetGroupPersonalHabitStatisticsResponse(value));
    }
}
