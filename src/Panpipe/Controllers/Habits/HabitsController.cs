using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Controllers.Helpers;
using Panpipe.Domain.HabitResult;
using Panpipe.Persistence;
using Panpipe.Persistence.Identity;

namespace Panpipe.Controllers.Habits;

[ApiController]
[Route("/api/v1.1/[controller]")]
[Authorize]
public class HabitsController(AppDbContext appDbContext, UserManager<AppIdentityUser> userManager): ControllerBase
{
    private readonly AppDbContext _appDbContext = appDbContext;
    private readonly UserManager<AppIdentityUser> _userManager = userManager;

    [HttpGet]
    [Route("templates")]
    [TranslateResultToActionResult]
    public async Task<Result<GetPublicTemplatesResponse>> GetPublicTemplates()
    {
        var templates = await _appDbContext.HabitParamsSets
            .AsNoTracking()
            .Where(x => x.IsPublicTemplate)
            .Include(x => x.Goal)
            .Include(x => x.Tags)
            .ToListAsync();
        
        return Result.Success(new GetPublicTemplatesResponse(templates.Select(template => 
            new GetPublicTemplatesResponseTemplate(
                template.Id, 
                template.Name, 
                template.Description,
                template.Tags.Select(tag => new GetPublicTemplatesResponseTag(tag.Id, tag.Name)).ToList(),
                Periodicity.FromFrequency(template.Frequency),
                template.Goal.ToReadableString(), 
                template.ResultType.ToString()
            )
        ).ToList()));
    }

    [HttpGet]
    [Route("tags")]
    [TranslateResultToActionResult]
    public async Task<Result<GetTagsResponse>> GetAllTags()
    {
        var tags = await _appDbContext.Tags
            .AsNoTracking()
            .ToListAsync();

        return Result.Success(new GetTagsResponse(
            tags.Select(tag => new GetTagsResponseTag(tag.Id, tag.Name)).ToList()
        ));
    }

    [HttpGet]
    [Route("tags/{id:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetTagResponse>> GetTagById([FromRoute] Guid id)
    {
        var tag = await _appDbContext.Tags
            .AsNoTracking()
            .Where(tag => tag.Id == id)
            .FirstOrDefaultAsync();
        
        if (tag is null)
        {
            return Result.NotFound($"Tag with id {id} was not found");
        }

        return Result.Success(new GetTagResponse(tag.Name));
    }   

    [HttpPut]
    [Route("{habitId:guid}/marks/{markId:guid}/result")]
    [TranslateResultToActionResult]
    public async Task<Result> ChangeHabitResult(
        [FromRoute] Guid habitId, [FromRoute] Guid markId, [FromBody] ChangeHabitResultRequest request
    )
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim");
        }

        var habitWithHabitParamsSet = await _appDbContext.Habits
            .Where(habit => habit.Id == habitId)
            .Include(habit => habit.Marks)
                .ThenInclude(habit => habit.Result)
            .Join(
                _appDbContext.HabitParamsSets
                    .Include(habitParamsSet => habitParamsSet.Goal),
                habit => habit.ParamsSetId,
                paramsSet => paramsSet.Id,
                (habit, paramsSet) => new 
                {
                    habit,
                    paramsSet
                }
            )
            .FirstOrDefaultAsync();
        
        if (habitWithHabitParamsSet is null)
        {
            var habitCollection = await _appDbContext.HabitCollections
                .Where(habitCollection => habitCollection.Id == habitId)
                .FirstOrDefaultAsync();
            
            if (habitCollection is null)
            {
                return Result.NotFound($"Habit with id {habitId} or its' params set is not found");
            }
            
            habitWithHabitParamsSet = await _appDbContext.GroupUserHabitOwners
                .Where(habitOwner => 
                    habitOwner.UserId == user.Id && habitCollection.HabitIds.Contains(habitOwner.HabitId))
                .Join(
                    _appDbContext.Habits
                        .Include(habit => habit.Marks)
                            .ThenInclude(habitMark => habitMark.Result),
                    habitOwner => habitOwner.HabitId,
                    habit => habit.Id,
                    (habitOwner, habit) => new 
                    {
                        habit
                    }
                )
                .Join(
                    _appDbContext.HabitParamsSets
                        .Include(habitParamsSet => habitParamsSet.Goal),
                    habitInfo => habitInfo.habit.ParamsSetId,
                    paramsSet => paramsSet.Id,
                    (habitInfo, paramsSet) => new 
                    {
                        habitInfo.habit,
                        paramsSet
                    }
                )
                .FirstOrDefaultAsync();
            
            if (habitWithHabitParamsSet is null)
            {
                return Result.CriticalError(
                    $"Cannot find personal habit for user with id {user.Id} for group habit with id {habitId}"
                );
            }
        }

        var habit = habitWithHabitParamsSet.habit;
        var paramsSet = habitWithHabitParamsSet.paramsSet;

        if (!paramsSet.ResultType.TryParse(request.Value, request.Comment, out var newResult))
        {
            return Result.Invalid(new ValidationError(
                $"Cannot parse string \'{request.Value}\' into habit result type \'{paramsSet.ResultType}\'"
            ));
        }

        _appDbContext.AbstractHabitResults.Add(newResult);

        var result = habit.ChangeResult(markId, newResult);

        if (result.IsSuccess)
        {
            await _appDbContext.SaveChangesAsync();
        }

        return result;
    }

    [HttpPut]
    [Route("{habitId:guid}/parameters")]
    [TranslateResultToActionResult]
    public async Task<Result> ChangeHabitParams(
        [FromRoute] Guid habitId,
        [FromQuery] string? name,
        [FromQuery] string? description,
        [FromQuery] string? goal
    )
    {
        var habitParamsSet = await _appDbContext.Habits
            .Where(habit => habit.Id == habitId)
            .Join(
                _appDbContext.HabitParamsSets
                    .Include(habit => habit.Goal),
                habit => habit.ParamsSetId,
                paramsSet => paramsSet.Id,
                (habit, paramsSet) => new 
                {
                    paramsSet
                }
            )
            .FirstOrDefaultAsync();
        
        if (habitParamsSet is null)
        {
            var habitCollection = await _appDbContext.HabitCollections
                .Where(habitCollection => habitCollection.Id == habitId)
                .FirstOrDefaultAsync();
            
            if (habitCollection is null)
            {
                return Result.NotFound($"Habit with id {habitId} or its' params set is not found");
            }
            
            habitParamsSet = await _appDbContext.HabitParamsSets
                .Include(habitParamsSet => habitParamsSet.Goal)
                .Where(habitParamsSet => habitParamsSet.Id == habitCollection.ParamsSetId)
                .Select(habitParamsSet => new { paramsSet = habitParamsSet })
                .FirstOrDefaultAsync();
            
            if (habitParamsSet is null)
            {
                return Result.CriticalError(
                    $"Cannot find habit params set with id {habitCollection.ParamsSetId} " +
                    $"for group habit with id {habitId}"
                );
            }
        }

        var paramsSet = habitParamsSet.paramsSet;

        if (paramsSet.IsPublicTemplate)
        {
            return Result.Invalid(new ValidationError(
                $"Cannot change parameters of templated habit with id {habitId}"
            ));
        }

        if (name is not null)
        {
            paramsSet.SetName(name);
        }

        if (description is not null)
        {
            paramsSet.SetDescription(description);
        }

        if (goal is not null)
        {
            const string GoalComment = "";
            
            var parseResult = paramsSet.ResultType.TryParse(goal, GoalComment, out var newGoal);

            if (!parseResult)
            {
                return Result.Invalid(new ValidationError($"Cannot parse goal \"{goal}\""));
            }

            paramsSet.SetGoal(newGoal);

            _appDbContext.AbstractHabitResults.Add(newGoal);
        }

        await _appDbContext.SaveChangesAsync();

        return Result.Success();
    }
}
