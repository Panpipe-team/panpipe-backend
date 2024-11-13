using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Application.Commands.ChangeHabitResult;
using Panpipe.Application.Commands.CreateUserHabitFromParams;
using Panpipe.Application.Queries.GetHabit;
using Panpipe.Application.Queries.GetHabits;
using Panpipe.Application.Queries.GetHabitTemplates;
using Panpipe.Persistence.Identity;
using Panpipe.Presentation.Helpers;
using Panpipe.Presentation.Requests;
using Panpipe.Presentation.Responses;

namespace Panpipe.Presentation.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
[Authorize]
public class HabitsController: ControllerBase
{
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly IMediator _mediator;

    public HabitsController(IMediator mediator, UserManager<AppIdentityUser> userManager) 
    {
        _mediator = mediator;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("templates")]
    public async Task<ActionResult<GetHabitTemplatesResponse>> GetHabitTemplates()
    {
        var query = new GetHabitTemplatesQuery();
        var result = await _mediator.Send(query);

        return result.Map(
            habitParamsList => new GetHabitTemplatesResponse(
                habitParamsList.Select(habitParam => new GetHabitTemplatesResponseTemplate(
                    habitParam.Id, 
                    habitParam.Name, 
                    habitParam.Periodicity.GetReadable(), 
                    habitParam.Goal.GetReadableValue(), 
                    habitParam.ResultType.GetReadable()
                )).ToList()
            )
        ).ToActionResult(this);
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<GetHabitResponse>> GetHabitById([FromRoute] Guid id)
    {
        var query = new GetHabitQuery(id);
        var result = await _mediator.Send(query);

        var habit = result.Value.Item1;
        var habitParams = result.Value.Item2;

        return Result.Success(new GetHabitResponse(
            habitParams.Name,
            habitParams.Periodicity.GetReadable(),
            habitParams.Goal.GetReadableValue(),
            habitParams.ResultType.GetReadable(),
            habit.HabitMarks.Select(mark => new GetHabitResponseMark(
                mark.Id,
                mark.Timestamp.UtcDateTime,
                mark.Result is null ? null : new GetHabitResponseResult(mark.Result.GetReadableValue())
            )).ToList()
        )).ToActionResult(this);
    }

    [HttpGet]
    public async Task<ActionResult<GetHabitsResponse>> GetHabits()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim").ToActionResult(this);
        }

        var query = new GetHabitsQuery(user.Id);
        var result = await _mediator.Send(query);

        return result.Map(habit => new GetHabitsResponse(
            result.Value.Select(tuple => new GetHabitsResponseHabit(
                tuple.Item1.Id,
                tuple.Item2.Name,
                tuple.Item2.Periodicity.GetReadable(),
                tuple.Item2.Goal.GetReadableValue(),
                tuple.Item2.ResultType.GetReadable()
            )).ToList()
        )).ToActionResult(this);
    }

    [HttpPost]
    public async Task<ActionResult<CreateHabitResponse>> CreateHabit([FromBody] CreateHabitRequest request)
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.Unauthorized("Cannot find authorized user by claim").ToActionResult(this);
        }

        var command = new CreateUserHabitFromParamsCommand(user.Id, request.TemplateId);
        var result = await _mediator.Send(command);

        return result.ToActionResult(this);
    }

    [HttpPut]
    [Route("{habitId:guid}/marks/{markId:guid}/result")]
    public async Task<ActionResult> ChangeHabitResult(
        [FromRoute] Guid habitId, 
        [FromRoute] Guid markId, 
        [FromBody] ReplaceHabitResultRequest request
    )
    {
        var command = new ChangeHabitResultCommand(habitId, markId, request.Value);
        var result = await _mediator.Send(command);

        return result.ToActionResult(this);
    }
}
