using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Panpipe.Persistence.Identity;
using Panpipe.Presentation.Requests;
using Panpipe.Presentation.Responses;

namespace Panpipe.Presentation.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class HabitsController
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
        throw new NotImplementedException();
    }

    [HttpGet]
    [Route("{id:guid}")]
    public async Task<ActionResult<GetHabitResponse>> GetHabitById([FromRoute] Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<ActionResult<GetHabitsResponse>> GetHabits()
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<CreateHabitResponse>> CreateHabit([FromBody] CreateHabitRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpPut]
    [Route("{habitId:guid}/marks/{markId:guid}/result")]
    public async Task<ActionResult> ReplaceHabitResult(
        [FromRoute] Guid habitId, 
        [FromRoute] Guid markId, 
        [FromBody] ReplaceHabitResultRequest request
    )
    {
        throw new NotImplementedException();
    }
}
