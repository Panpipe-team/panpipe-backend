using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Controllers.Habits.Helpers;
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
        // FAKED
        return Result.Success(new GetTagsResponse(
            [
                new GetTagsResponseTag(Guid.Empty, "Faked tag name")
            ]
        ));
    }

    [HttpGet]
    [Route("tags/{id:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetTagResponse>> GetTagById([FromRoute] Guid id)
    {
        // FAKED
        return Result.Success(new GetTagResponse("Faked tag name 2"));
    }   

    [HttpPut]
    [Route("{habitId:guid}/marks/{markId:guid}/result")]
    [TranslateResultToActionResult]
    public async Task<Result> ChangeHabitResult(
        [FromRoute] Guid habitId, [FromRoute] Guid markId, [FromBody] ChangeHabitResultRequest request
    )
    {
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
            return Result.NotFound($"Habit with id {habitId} or its' params set is not found");
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
    public async Task<Result> ChangeHabitParams(
        [FromRoute] Guid habitId,
        [FromQuery] string? name,
        [FromQuery] string? description,
        [FromQuery] string? periodicityType,
        [FromQuery] int? periodicityValue,
        [FromQuery] string? goal
    )
    {
        // Faked
        return Result.Success();
    }
}
