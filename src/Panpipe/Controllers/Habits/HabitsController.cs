using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Panpipe.Controllers.Habits.Helpers;
using Panpipe.Domain.Habit;
using Panpipe.Domain.HabitOwner;
using Panpipe.Domain.HabitParamsSet;
using Panpipe.Domain.HabitResult;
using Panpipe.Persistence;

namespace Panpipe.Controllers.Habits;

[ApiController]
[Route("/api/v1/[controller]")]
public class HabitsController(AppDbContext appDbContext): ControllerBase
{
    private readonly AppDbContext _appDbContext = appDbContext;

    [HttpGet]
    [Route("templates")]
    public async Task<Result<GetPublicTemplatesResponse>> GetPublicTemplates()
    {
        var templates = await _appDbContext.HabitParamsSets
            .AsNoTracking()
            .Where(x => x.IsPublicTemplate)
            .Include(x => x.Goal)
            .ToListAsync();
        
        return Result.Success(new GetPublicTemplatesResponse(templates.Select(template => 
            new GetPublicTemplatesResponseTemplate(
                template.Id, 
                template.Name, 
                template.Frequency.ToReadableString(), 
                template.Goal.ToReadableString(), 
                template.ResultType.ToString()
            )
        ).ToList()));
    }

    [HttpGet]
    [Route("{id:guid}")]
    [TranslateResultToActionResult]
    public async Task<Result<GetHabitResponse>> GetHabit([FromRoute] Guid id)
    {
        var result = await _appDbContext.Habits
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Include(x => x.Marks)
                .ThenInclude(x => x.Result)
            .Join(
                _appDbContext.HabitParamsSets
                    .AsNoTracking()
                    .Include(x => x.Goal),
                habit => habit.ParamsSetId,
                paramsSet => paramsSet.Id,
                (habit, paramsSet) => new 
                {
                    paramsSet.Name,
                    paramsSet.Frequency,
                    paramsSet.Goal,
                    paramsSet.ResultType,
                    habit.Marks
                }
            )
            .FirstOrDefaultAsync();
        
        if (result is null)
        {
            return Result.NotFound();
        }
        
        return Result.Success(new GetHabitResponse(
            result.Name,
            result.Frequency.ToReadableString(),
            result.Goal.ToReadableString(),
            result.ResultType.ToString(),
            result.Marks.Select(mark => new GetHabitResponseMark(
                mark.Id, 
                mark.TimestampUtc.UtcDateTime,
                mark.Result is null ? null : new GetHabitResponseMarkResult(mark.Result.ToReadableString()) 
            )).ToList()
        ));
    }

    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<Result<GetHabitsResponse>> GetHabits()
    {
        var userId = Guid.Empty;

        var habitIds = await _appDbContext.UserHabitOwners
            .AsNoTracking()
            .Where(x => x.UserId == userId)
            .Select(x => x.HabitId)
            .ToListAsync();
        
        var habits = await _appDbContext.Habits
            .AsNoTracking()
            .Where(x => habitIds.Contains(x.Id))
            .ToListAsync();
        
        var habitParamsSetIds = habits.Select(x => x.ParamsSetId).ToList();

        var habitParamsSets = await _appDbContext.HabitParamsSets
            .AsNoTracking()
            .Where(x => habitParamsSetIds.Contains(x.Id))
            .Include(x => x.Goal)
            .ToListAsync();

        var result = new List<Tuple<Habit, HabitParamsSet>>();

        foreach (var habit in habits)
        {
            var habitParamsSet = habitParamsSets.FirstOrDefault(x => x.Id == habit.ParamsSetId);
            
            if (habitParamsSet is null)
            {
                return Result.CriticalError(
                    $"For found habit with id {habit.Id} was not found habit params set with id {habit.ParamsSetId}"
                );
            }

            result.Add(new (habit, habitParamsSet));
        }

        return Result.Success(new GetHabitsResponse(
            result.Select(habitInfo => new GetHabitsResponseHabit(
                habitInfo.Item1.Id,
                habitInfo.Item2.Name,
                habitInfo.Item2.Frequency.ToReadableString(),
                habitInfo.Item2.Goal.ToReadableString(),
                habitInfo.Item2.ResultType.ToString()
            )).ToList()
        ));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    public async Task<Result<CreateHabitResponse>> CreateHabit([FromBody] CreateHabitRequest request)
    {
        var userId = Guid.Empty;

        var habitParamsSet = await _appDbContext.HabitParamsSets
            .AsNoTracking()
            .Where(x => x.Id == request.TemplateId)
            .FirstOrDefaultAsync();

        if (habitParamsSet is null)
        {
            return Result.Invalid(new ValidationError($"Not found habit params template with id {request.TemplateId}"));
        }

        var habit = new Habit(Guid.NewGuid(), request.TemplateId);

        _appDbContext.Habits.Add(habit);

        var userHabitOwner = new UserHabitOwner(Guid.NewGuid(), userId, habit.Id);

        _appDbContext.UserHabitOwners.Add(userHabitOwner);

        var emptyMarksTimestamps = habitParamsSet.CalculateTimestampsOfEmptyMarksForNewlyCreatedHabit();

        foreach (var timestamp in emptyMarksTimestamps)
        {
            var emptyMark = HabitMark.CreateEmpty(Guid.NewGuid(), timestamp);
            habit.AddEmptyMark(emptyMark);
        }

        await _appDbContext.SaveChangesAsync();

        return Result.Created(new CreateHabitResponse(habit.Id));
    }

    [HttpPut]
    [Route("{habitId:guid}/marks/{markId:guid}/result")]
    [TranslateResultToActionResult]
    public async Task<Result> ChangeHabitResult(
        [FromRoute] Guid habitId, [FromRoute] Guid markId, [FromBody] ChangeHabitResultRequest request
    )
    {
        var habit = await _appDbContext.Habits
            .Where(habit => habit.Id == habitId)
            .Include(habit => habit.Marks)
                .ThenInclude(x => x.Result)
            .FirstOrDefaultAsync();
        
        if (habit is null)
        {
            return Result.NotFound($"Habit with id {habitId} is not found");
        }

        var habitParamsSet = await _appDbContext.HabitParamsSets
            .AsNoTracking()
            .Where(habitParamsSet => habitParamsSet.Id == habit.ParamsSetId)
            .Include(habitParamsSet => habitParamsSet.Goal)
            .FirstOrDefaultAsync();
        
        if (habitParamsSet is null)
        {
            return Result.CriticalError(
                $"For found habit with id {habitId} habit params set with id {habit.ParamsSetId} was not found"
            );
        }

        if (!habitParamsSet.ResultType.TryParse(request.Value, out var newResult))
        {
            return Result.Invalid(new ValidationError(
                $"Cannot parse string \'{request.Value}\' into habit result type \'{habitParamsSet.ResultType}\'"
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
}