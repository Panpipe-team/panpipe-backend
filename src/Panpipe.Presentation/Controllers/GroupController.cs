using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Panpipe.Application.Commands.CreateGroup;
using Panpipe.Application.Queries.GetGroup;
using Panpipe.Persistence.Identity;
using Panpipe.Presentation.Requests;
using Ardalis.Result;
using Panpipe.Presentation.Responses;
using Microsoft.AspNetCore.Authorization;
using Panpipe.Application.Queries.GetGroups;

namespace Panpipe.Presentation.Controllers;

[ApiController]
[Route("/api/v1/groups")]
public class GroupController: ControllerBase {
    private readonly UserManager<AppIdentityUser> _userManager;
    private readonly IMediator _mediator;

    public GroupController(IMediator mediator, UserManager<AppIdentityUser> userManager) 
    {
        _mediator = mediator;
        _userManager = userManager;
    }

    [HttpGet]
    [Route("{id:Guid}")]
    [Authorize]
    public async Task<ActionResult<GetGroupResponse>> GetById([FromRoute] Guid id) 
    {
        var command = new GetGroupQuery(id);
        var group = await _mediator.Send(command);

        return group.Map(group => new GetGroupResponse(group.Name, group.UserIds.ToList())).ToActionResult(this);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<GetGroupsResponseGroup>>> GetAllByUser()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.CriticalError("Cannot find authorized user by claim").ToActionResult(this);
        }

        var command = new GetGroupsQuery(user.Id);
        var result = await _mediator.Send(command);

        return result.Map(groups => new List<GetGroupsResponseGroup>(
            groups.Select(group => new GetGroupsResponseGroup(group.Id, group.Name))
        )).ToActionResult(this);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CreateGroupResponse>> Create([FromBody] CreateGroupRequest request) 
    {
        var user = await _userManager.GetUserAsync(User);

        if (user is null)
        {
            return Result.CriticalError("Cannot find authorized user by claim").ToActionResult(this);
        }

        var command = new CreateGroupCommand(user.Id, request.Name);
        var result = await _mediator.Send(command);

        return result.Map(guid => new CreateGroupResponse(guid)).ToActionResult(this);
    }
}
